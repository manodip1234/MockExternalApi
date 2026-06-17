// =============================================================================
// FILE:      CallbackController.cs
// PURPOSE:   Per-department sync-result callbacks posted by RTPSWB.
//            Each dept has its own route and HMAC credentials.
//            GET    /{dept}/sync-response/received   — inspect last N payloads
//            DELETE /{dept}/sync-response/received   — clear log
// =============================================================================

using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace MockExternalApi.Controllers;

[ApiController]
public sealed class CallbackController : ControllerBase
{
    private const string TimestampFormat = "yyyy-MM-ddTHH:mm:ssZ";

    // Per-department in-memory logs (last 500 each)
    private static readonly Dictionary<string, List<(DateTime ReceivedAtUtc, string Body)>> Logs = new()
    {
        ["BCW"] = [],
        ["FSD"] = [],
        ["HFW"] = [],
        ["ABC"] = []
    };

    private static readonly object LockObj = new();

    private readonly IConfiguration _config;
    private readonly ILogger<CallbackController> _logger;

    public CallbackController(IConfiguration config, ILogger<CallbackController> logger)
    {
        _config = config;
        _logger = logger;
    }

    [HttpPost("bcw/sync-response")]
    public Task<IActionResult> BcwCallback()
        => HandleAsync(
            "BCW",
            _config["BcwApi:ApiKey"] ?? "bcw-test-api-key-001",
            _config["BcwApi:HmacSecret"] ?? "bcw-test-api-key-001");

    [HttpGet("bcw/sync-response/received")]
    public IActionResult BcwReceived([FromQuery] int take = 20) => GetReceived("BCW", take);

    [HttpDelete("bcw/sync-response/received")]
    public IActionResult BcwClear() => ClearLog("BCW");

    [HttpPost("food/sync-response")]
    public Task<IActionResult> FsdCallback()
        => HandleAsync(
            "FSD",
            _config["FoodApi:ApiKey"] ?? "food-api-key-001",
            _config["FoodApi:HmacSecret"] ?? "food-hmac-secret-001");

    [HttpGet("food/sync-response/received")]
    public IActionResult FsdReceived([FromQuery] int take = 20) => GetReceived("FSD", take);

    [HttpDelete("food/sync-response/received")]
    public IActionResult FsdClear() => ClearLog("FSD");

    [HttpPost("hfw/sync-response")]
    public Task<IActionResult> HfwCallback()
        => HandleAsync(
            "HFW",
            _config["TransApi:ApiKey"] ?? "hfw-api-key-001",
            _config["TransApi:HmacSecret"] ?? "hfw-hmac-secret-001");

    [HttpGet("hfw/sync-response/received")]
    public IActionResult HfwReceived([FromQuery] int take = 20) => GetReceived("HFW", take);

    [HttpDelete("hfw/sync-response/received")]
    public IActionResult HfwClear() => ClearLog("HFW");

    // ABC — sync result callbacks from RTPS (pipeline = 'ABC')
    [HttpPost("abc/sync-response")]
    public Task<IActionResult> AbcCallback()
        => HandleAsync(
            "ABC",
            _config["AbcApi:ApiKey"]     ?? "rtps-demo-key-001",
            _config["AbcApi:HmacSecret"] ?? "rtps-demo-secret-001");

    [HttpGet("abc/sync-response/received")]
    public IActionResult AbcReceived([FromQuery] int take = 20) => GetReceived("ABC", take);

    [HttpDelete("abc/sync-response/received")]
    public IActionResult AbcClear() => ClearLog("ABC");

    private async Task<IActionResult> HandleAsync(string dept, string expectedApiKey, string secret)
    {
        Request.EnableBuffering();
        using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
        var rawBody = await reader.ReadToEndAsync();
        Request.Body.Position = 0;

        var pathAndQuery = (Request.Path + Request.QueryString).ToString();
        var timestamp = Request.Headers.TryGetValue("X-TIMESTAMP", out var ts) ? ts.ToString() : string.Empty;
        var apiKey = Request.Headers.TryGetValue("X-API-KEY", out var ak) ? ak.ToString() : string.Empty;
        var incomingSig = Request.Headers.TryGetValue("X-HMAC-SIGNATURE", out var sig) ? sig.ToString() : string.Empty;
        var apiVersion = Request.Headers.TryGetValue("X-API-VERSION", out var av) ? av.ToString() : string.Empty;

        _logger.LogInformation("[{Dept}-CALLBACK] POST {Path} | Key={Key} | Ts={Ts}", dept, pathAndQuery, apiKey, timestamp);

        if (apiKey != expectedApiKey)
            return Unauthorized(new { error = $"{dept}: Invalid API key" });

        if (string.IsNullOrWhiteSpace(timestamp))
            return Unauthorized(new { error = $"{dept}: X-TIMESTAMP missing" });

        if (string.IsNullOrWhiteSpace(apiVersion))
            return Unauthorized(new { error = $"{dept}: X-API-VERSION missing" });

        if (!DateTime.TryParseExact(
                timestamp,
                TimestampFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out var parsedTs))
            return Unauthorized(new { error = $"{dept}: Invalid X-TIMESTAMP format (expected UTC 'Z')" });

        var driftMinutes = Math.Abs((DateTime.UtcNow - parsedTs.ToUniversalTime()).TotalMinutes);
        if (driftMinutes > 5)
            return StatusCode(403, new { error = $"{dept}: Timestamp expired" });

        if (string.IsNullOrWhiteSpace(incomingSig))
            return Unauthorized(new { error = $"{dept}: X-HMAC-SIGNATURE missing" });

        var rawString = $"POST|{pathAndQuery}|{timestamp}|{rawBody}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var expected = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(rawString)));

        if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(expected),
                Encoding.UTF8.GetBytes(incomingSig)))
            return Unauthorized(new { error = $"{dept}: Signature mismatch" });

        lock (LockObj)
        {
            var log = Logs[dept];
            log.Add((DateTime.UtcNow, rawBody));
            if (log.Count > 500)
                log.RemoveRange(0, log.Count - 500);
        }

        try
        {
            var el = JsonSerializer.Deserialize<JsonElement>(rawBody);
            var pretty = JsonSerializer.Serialize(el, new JsonSerializerOptions { WriteIndented = true });
            _logger.LogInformation("[{Dept}-CALLBACK] Accepted\n{Body}", dept, pretty);
        }
        catch
        {
            _logger.LogInformation("[{Dept}-CALLBACK] Accepted (raw): {Body}", dept, rawBody);
        }

        return Ok(new { received = true, dept, timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") });
    }

    private IActionResult GetReceived(string dept, int take)
    {
        take = Math.Clamp(take, 1, 200);
        List<object> items;
        lock (LockObj)
        {
            items = Logs[dept]
                .OrderByDescending(x => x.ReceivedAtUtc)
                .Take(take)
                .Select(x => (object)new { received_at_utc = x.ReceivedAtUtc, body = x.Body })
                .ToList();
        }
        return Ok(new { dept, count = items.Count, items });
    }

    private IActionResult ClearLog(string dept)
    {
        lock (LockObj) { Logs[dept].Clear(); }
        _logger.LogInformation("[{Dept}-CALLBACK] In-memory log cleared.", dept);
        return Ok(new { cleared = true, dept });
    }
}

