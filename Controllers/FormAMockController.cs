// =============================================================================
// FILE:      FormAMockController.cs
// PURPOSE:   Mock Form A (Monthly MIS) submission endpoint for FSD and BCW.
//            Uses REAL department_id / service_id / office_id from rtps_wb.
//
// DB ground truth (as of current schema):
//   FSD  dept_id=1   services: 1,2         offices: 1,2,3,4
//   BCW  dept_id=3   services: 3,14,15,16  offices: 45,46,47,48
//
// Route: GET /mock-api/form-a/submissions?dept=FSD&page=1&page_size=100
// Auth:  HMAC  (X-API-KEY + X-TIMESTAMP + X-HMAC-SIGNATURE)
// =============================================================================

using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace MockExternalApi.Controllers;

[ApiController]
[Route("mock-api/form-a")]
public sealed class FormAMockController : ControllerBase
{
    private const string ValidApiKey  = "rtps-demo-key-001";
    private const string HmacSecret   = "rtps-demo-secret-001";

    private readonly ILogger<FormAMockController> _logger;

    public FormAMockController(ILogger<FormAMockController> logger)
        => _logger = logger;

    // ── FSD Monthly MIS ──────────────────────────────────────────────────────
    // dept_id=1, services 1 & 2, offices 1-4
    [HttpGet("fsd/submissions")]
    public IActionResult GetFsdSubmissions(
        [FromQuery] int page = 1, [FromQuery] int page_size = 100)
    {
        var auth = ValidateHmac(Request.Path + Request.QueryString);
        if (auth != null) return auth;

        var all = BuildFormAData("FSD", 1,
            serviceIds: [1, 2],
            officeIds:  [1, 2, 3, 4]);

        var (paged, total) = Paginate(all, page, page_size);
        _logger.LogInformation("[FORM-A-FSD] GET submissions page={Page} -> {Count}/{Total}", page, paged.Count, total);
        return Ok(Envelope(paged, total, "FSD Form A MIS data"));
    }

    // ── BCW Monthly MIS ───────────────────────────────────────────────────────
    // dept_id=3, services 3,14,15,16, offices 45,46,47,48
    [HttpGet("bcw/submissions")]
    public IActionResult GetBcwSubmissions(
        [FromQuery] int page = 1, [FromQuery] int page_size = 100)
    {
        var auth = ValidateHmac(Request.Path + Request.QueryString);
        if (auth != null) return auth;

        var all = BuildFormAData("BCW", 3,
            serviceIds: [3, 14, 15, 16],
            officeIds:  [45, 46, 47, 48]);

        var (paged, total) = Paginate(all, page, page_size);
        _logger.LogInformation("[FORM-A-BCW] GET submissions page={Page} -> {Count}/{Total}", page, paged.Count, total);
        return Ok(Envelope(paged, total, "BCW Form A MIS data"));
    }

    // ── Callback receivers ────────────────────────────────────────────────────
    [HttpPost("fsd/sync-response")]
    public IActionResult FsdCallback() => LogCallback("FORM-A-FSD");

    [HttpPost("bcw/sync-response")]
    public IActionResult BcwCallback() => LogCallback("FORM-A-BCW");

    // ─────────────────────────────────────────────────────────────────────────
    private static List<FormARecordDto> BuildFormAData(
        string deptCode, int deptId,
        int[] serviceIds, int[] officeIds)
    {
        var records = new List<FormARecordDto>();
        var now = DateTime.UtcNow;

        // Two months: previous + current
        var periods = new[]
        {
            (Month: now.Month == 1 ? 12 : now.Month - 1,
             Year:  now.Month == 1 ? now.Year - 1 : now.Year),
            (Month: now.Month, Year: now.Year)
        };

        foreach (var (month, year) in periods)
        {
            foreach (var svcId in serviceIds)
            {
                foreach (var offId in officeIds)
                {
                    var seed = month * 1000 + svcId * 10 + offId;
                    var received = 30 + (seed % 40);
                    var form1    = received - (seed % 6);
                    var within   = form1 - (seed % 5);
                    var after    = seed % 4;
                    var pending  = Math.Max(0, received - within - after);

                    records.Add(new FormARecordDto
                    {
                        dept_code            = deptCode,
                        department_id        = deptId,
                        office_id            = offId,
                        service_id           = svcId,
                        period_month         = month,
                        period_year          = year,
                        applications_received = received,
                        form1_issued         = form1,
                        disposed_within_time = within,
                        disposed_after_time  = after,
                        pending_applications = pending
                    });
                }
            }
        }

        return records;
    }

    private IActionResult LogCallback(string tag)
    {
        _logger.LogInformation("[{Tag}] Callback received from RTPS", tag);
        return Ok(new { received = true, timestamp = DateTime.UtcNow });
    }

    private static object Envelope<T>(List<T> data, int total, string msg) => new
    {
        success    = true,
        message    = msg,
        payload_id = $"P-FORMA-{DateTime.UtcNow:yyyyMMddHH}",
        totalCount = total,
        data
    };

    private static (List<T> Page, int Total) Paginate<T>(List<T> all, int page, int pageSize)
    {
        pageSize = Math.Clamp(pageSize, 1, 500);
        page     = Math.Max(1, page);
        return (all.Skip((page - 1) * pageSize).Take(pageSize).ToList(), all.Count);
    }

    private IActionResult? ValidateHmac(string pathAndQuery)
    {
        if (!Request.Headers.TryGetValue("X-API-KEY", out var key) || key != ValidApiKey)
            return Unauthorized(new { error = "Invalid or missing X-API-KEY" });

        if (!Request.Headers.TryGetValue("X-TIMESTAMP", out var ts) || string.IsNullOrWhiteSpace(ts))
            return Unauthorized(new { error = "X-TIMESTAMP missing" });

        if (!Request.Headers.TryGetValue("X-HMAC-SIGNATURE", out var sig) || string.IsNullOrWhiteSpace(sig))
            return Unauthorized(new { error = "X-HMAC-SIGNATURE missing" });

        var raw      = $"GET|{pathAndQuery}|{ts}|";
        using var h  = new HMACSHA256(Encoding.UTF8.GetBytes(HmacSecret));
        var expected = Convert.ToBase64String(h.ComputeHash(Encoding.UTF8.GetBytes(raw)));

        if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(expected),
                Encoding.UTF8.GetBytes(sig.ToString())))
            return Unauthorized(new { error = "Signature mismatch" });

        return null;
    }

    public sealed class FormARecordDto
    {
        [JsonPropertyName("dept_code")]             public string dept_code             { get; set; } = default!;
        [JsonPropertyName("department_id")]         public int    department_id         { get; set; }
        [JsonPropertyName("office_id")]             public int?   office_id             { get; set; }
        [JsonPropertyName("service_id")]            public int    service_id            { get; set; }
        [JsonPropertyName("period_month")]          public int    period_month          { get; set; }
        [JsonPropertyName("period_year")]           public int    period_year           { get; set; }
        [JsonPropertyName("applications_received")] public int    applications_received { get; set; }
        [JsonPropertyName("form1_issued")]          public int    form1_issued          { get; set; }
        [JsonPropertyName("disposed_within_time")]  public int    disposed_within_time  { get; set; }
        [JsonPropertyName("disposed_after_time")]   public int    disposed_after_time   { get; set; }
        [JsonPropertyName("pending_applications")]  public int    pending_applications  { get; set; }
    }
}
