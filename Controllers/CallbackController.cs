// =============================================================================
// FILE:      CallbackController.cs
// PURPOSE:   Per-department sync-result callbacks posted by RTPSWB.
//            Each dept has its own route and HMAC credentials.
//            GET  /{dept}/sync-response/received  — inspect last N payloads
//            DELETE /{dept}/sync-response/received — clear log
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MockExternalApi.Controllers
{
    [ApiController]
    public sealed class CallbackController : ControllerBase
    {
        // Per-department in-memory logs (last 500 each)
        private static readonly Dictionary<string, List<(DateTime ReceivedAtUtc, string Body)>> _logs = new()
        {
            ["BCW"]  = new(),
            ["FSD"]  = new(),
            ["HFW"]  = new(),
        };
        private static readonly object _lock = new();

        private readonly IConfiguration _config;
        private readonly ILogger<CallbackController> _logger;

        public CallbackController(IConfiguration config, ILogger<CallbackController> logger)
        {
            _config = config;
            _logger = logger;
        }

        // ── BCW ───────────────────────────────────────────────────────────────

        [HttpPost("bcw/sync-response")]
        public Task<IActionResult> BcwCallback()
            => HandleAsync("BCW",
                _config["BcwApi:ApiKey"]     ?? "bcw-test-api-key-001",
                _config["BcwApi:HmacSecret"] ?? "bcw-test-api-key-001");

        [HttpGet("bcw/sync-response/received")]
        public IActionResult BcwReceived([FromQuery] int take = 20) => GetReceived("BCW", take);

        [HttpDelete("bcw/sync-response/received")]
        public IActionResult BcwClear() => ClearLog("BCW");

        // ── FSD ───────────────────────────────────────────────────────────────

        [HttpPost("food/sync-response")]
        public Task<IActionResult> FsdCallback()
            => HandleAsync("FSD",
                _config["FoodApi:ApiKey"]     ?? "food-api-key-001",
                _config["FoodApi:HmacSecret"] ?? "food-hmac-secret-001");

        [HttpGet("food/sync-response/received")]
        public IActionResult FsdReceived([FromQuery] int take = 20) => GetReceived("FSD", take);

        [HttpDelete("food/sync-response/received")]
        public IActionResult FsdClear() => ClearLog("FSD");

        // ── HFW ───────────────────────────────────────────────────────────────

        [HttpPost("hfw/sync-response")]
        public Task<IActionResult> HfwCallback()
            => HandleAsync("HFW",
                _config["TransApi:ApiKey"]     ?? "hfw-api-key-001",
                _config["TransApi:HmacSecret"] ?? "hfw-hmac-secret-001");

        [HttpGet("hfw/sync-response/received")]
        public IActionResult HfwReceived([FromQuery] int take = 20) => GetReceived("HFW", take);

        [HttpDelete("hfw/sync-response/received")]
        public IActionResult HfwClear() => ClearLog("HFW");

        // ── Shared handler ────────────────────────────────────────────────────

        private async Task<IActionResult> HandleAsync(string dept, string expectedApiKey, string secret)
        {
            Request.EnableBuffering();
            using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
            var rawBody = await reader.ReadToEndAsync();
            Request.Body.Position = 0;

            var path        = Request.Path.ToString();
            var timestamp   = Request.Headers.TryGetValue("X-TIMESTAMP",      out var ts)  ? ts.ToString()  : string.Empty;
            var apiKey      = Request.Headers.TryGetValue("X-API-KEY",        out var ak)  ? ak.ToString()  : string.Empty;
            var incomingSig = Request.Headers.TryGetValue("X-HMAC-SIGNATURE", out var sig) ? sig.ToString() : string.Empty;

            _logger.LogInformation("[{Dept}-CALLBACK] POST {Path} | Key={Key} | Ts={Ts}", dept, path, apiKey, timestamp);

            if (apiKey != expectedApiKey)
            {
                _logger.LogWarning("[{Dept}-CALLBACK] Rejected — invalid X-API-KEY", dept);
                return Unauthorized(new { error = $"{dept}: Invalid API key" });
            }

            if (string.IsNullOrWhiteSpace(timestamp))
            {
                _logger.LogWarning("[{Dept}-CALLBACK] Rejected — missing X-TIMESTAMP", dept);
                return Unauthorized(new { error = $"{dept}: X-TIMESTAMP missing" });
            }

            if (DateTime.TryParse(timestamp, null, System.Globalization.DateTimeStyles.RoundtripKind, out var parsedTs))
            {
                var drift = Math.Abs((DateTime.UtcNow - parsedTs.ToUniversalTime()).TotalMinutes);
                if (drift > 5)
                {
                    _logger.LogWarning("[{Dept}-CALLBACK] Rejected — timestamp expired (drift={Drift:F1}m)", dept, drift);
                    return StatusCode(403, new { error = $"{dept}: Timestamp expired" });
                }
            }

            if (string.IsNullOrWhiteSpace(incomingSig))
            {
                _logger.LogWarning("[{Dept}-CALLBACK] Rejected — missing X-HMAC-SIGNATURE", dept);
                return Unauthorized(new { error = $"{dept}: X-HMAC-SIGNATURE missing" });
            }

            var rawString  = $"POST|{path}|{timestamp}|{rawBody}";
            var keyBytes   = Encoding.UTF8.GetBytes(secret);
            var msgBytes   = Encoding.UTF8.GetBytes(rawString);
            using var hmac = new HMACSHA256(keyBytes);
            var expected   = Convert.ToBase64String(hmac.ComputeHash(msgBytes));

            if (!CryptographicOperations.FixedTimeEquals(
                    Encoding.UTF8.GetBytes(expected),
                    Encoding.UTF8.GetBytes(incomingSig)))
            {
                _logger.LogWarning("[{Dept}-CALLBACK] Rejected — HMAC mismatch", dept);
                return Unauthorized(new { error = $"{dept}: Signature mismatch" });
            }

            lock (_lock)
            {
                var log = _logs[dept];
                log.Add((DateTime.UtcNow, rawBody));
                if (log.Count > 500)
                    log.RemoveRange(0, log.Count - 500);
            }

            try
            {
                var el     = JsonSerializer.Deserialize<JsonElement>(rawBody);
                var pretty = JsonSerializer.Serialize(el, new JsonSerializerOptions { WriteIndented = true });
                _logger.LogInformation("[{Dept}-CALLBACK] ✓ Accepted\n{Body}", dept, pretty);
            }
            catch
            {
                _logger.LogInformation("[{Dept}-CALLBACK] ✓ Accepted (raw): {Body}", dept, rawBody);
            }

            return Ok(new { received = true, dept, timestamp = DateTime.UtcNow });
        }

        private IActionResult GetReceived(string dept, int take)
        {
            take = Math.Clamp(take, 1, 200);
            List<object> items;
            lock (_lock)
            {
                items = _logs[dept]
                    .OrderByDescending(x => x.ReceivedAtUtc)
                    .Take(take)
                    .Select(x => (object)new { received_at_utc = x.ReceivedAtUtc, body = x.Body })
                    .ToList();
            }
            return Ok(new { dept, count = items.Count, items });
        }

        private IActionResult ClearLog(string dept)
        {
            lock (_lock) { _logs[dept].Clear(); }
            _logger.LogInformation("[{Dept}-CALLBACK] In-memory log cleared.", dept);
            return Ok(new { cleared = true, dept });
        }
    }
}
