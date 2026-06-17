// =============================================================================
// FILE:      FormBMockController.cs
// PURPOSE:   Mock Form B (First Appeals) endpoint for BCW.
//            Uses REAL service_id and office_id from rtps_wb.
//
// DB ground truth:
//   BCW  dept_id=3
//   Services: 3 (Caste Cert), 14 (BCW Caste Cert), 15 (Welfare Scheme), 16 (Scholarship)
//   Offices:  45 (BCW Dist Kolkata), 46 (BCW State), 47 (BCW Dist Bardhaman), 48 (BCW Block N24P)
//   Officers: officer_id=5 (aotest, office 47), officer_id=4 (rotest, office 45)
//
// Route: GET /mock-api/form-b/bcw/submissions?page=1&page_size=100
// Auth:  HMAC
// =============================================================================

using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace MockExternalApi.Controllers;

[ApiController]
[Route("mock-api/form-b")]
public sealed class FormBMockController : ControllerBase
{
    private const string ValidApiKey = "rtps-demo-key-001";
    private const string HmacSecret  = "rtps-demo-secret-001";

    private readonly ILogger<FormBMockController> _logger;

    public FormBMockController(ILogger<FormBMockController> logger)
        => _logger = logger;

    // BCW Form B — First Appeals
    // Appellate Officer: officer_id=5 (aotest@gmail.com), office_id=47
    [HttpGet("bcw/submissions")]
    public IActionResult GetBcwSubmissions(
        [FromQuery] int page = 1, [FromQuery] int page_size = 100)
    {
        var auth = ValidateHmac(Request.Path + Request.QueryString);
        if (auth != null) return auth;

        var all = BuildFormBData();
        var (paged, total) = Paginate(all, page, page_size);
        _logger.LogInformation("[FORM-B-BCW] GET submissions page={Page} -> {Count}/{Total}", page, paged.Count, total);
        return Ok(Envelope(paged, total, "BCW Form B appeal data"));
    }

    [HttpPost("bcw/sync-response")]
    public IActionResult BcwCallback()
    {
        _logger.LogInformation("[FORM-B-BCW] Callback received from RTPS");
        return Ok(new { received = true, timestamp = DateTime.UtcNow });
    }

    // ─────────────────────────────────────────────────────────────────────────
    private static List<FormBRecordDto> BuildFormBData()
    {
        var statuses = new[] { "PENDING", "IN_PROGRESS", "DISPOSED", "REJECTED" };
        var now      = DateTime.UtcNow;

        // service_ids and office_ids anchored to real DB rows
        var entries = new[]
        {
            (svcId: 3,  offId: 45, aoEmail: "aotest@gmail.com"),
            (svcId: 14, offId: 47, aoEmail: "aotest@gmail.com"),
            (svcId: 15, offId: 45, aoEmail: "aotest@gmail.com"),
            (svcId: 16, offId: 48, aoEmail: "aotest@gmail.com"),
            (svcId: 3,  offId: 46, aoEmail: "aotest@gmail.com"),
            (svcId: 14, offId: 48, aoEmail: "aotest@gmail.com"),
        };

        return entries.SelectMany((e, i) =>
            Enumerable.Range(1, 3).Select(j =>
            {
                var idx     = i * 3 + j;
                var applied = now.AddDays(-(idx * 7));
                return new FormBRecordDto
                {
                    appeal_no            = $"BCW/B/2026/{idx:D5}",
                    application_no       = $"APP/BCW/2026/{idx:D5}",
                    dept_code            = "BCW",
                    department_id        = 3,
                    service_id           = e.svcId,
                    office_id            = e.offId,
                    appellate_officer_email = e.aoEmail,
                    applicant_name       = ApplicantName(idx),
                    applicant_mobile     = $"940050{idx:D4}",
                    present_status       = statuses[(idx - 1) % statuses.Length],
                    applied_date         = applied.ToString("yyyy-MM-dd"),
                    last_updated_date    = applied.AddDays(3).ToString("yyyy-MM-dd"),
                    period_month         = applied.Month,
                    period_year          = applied.Year
                };
            })
        ).ToList();
    }

    private IActionResult LogCallback(string tag)
    {
        _logger.LogInformation("[{Tag}] Callback received", tag);
        return Ok(new { received = true });
    }

    private static object Envelope<T>(List<T> data, int total, string msg) => new
    {
        success    = true,
        message    = msg,
        payload_id = $"P-FORMB-{DateTime.UtcNow:yyyyMMddHH}",
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

    private static string ApplicantName(int i) => i switch
    {
        1 => "Ratan Bauri",   2 => "Gita Pal",       3 => "Md. Sohel Rana",
        4 => "Chhanda Mahato",5 => "Nur Alam Shaikh", 6 => "Susmita Bose",
        7 => "Khokan Das",    8 => "Sabina Khatun",   9 => "Tapas Roy",
        _ => $"Applicant {i}"
    };

    public sealed class FormBRecordDto
    {
        [JsonPropertyName("appeal_no")]               public string  appeal_no               { get; set; } = default!;
        [JsonPropertyName("application_no")]          public string? application_no          { get; set; }
        [JsonPropertyName("dept_code")]               public string  dept_code               { get; set; } = default!;
        [JsonPropertyName("department_id")]           public int     department_id           { get; set; }
        [JsonPropertyName("service_id")]              public int     service_id              { get; set; }
        [JsonPropertyName("office_id")]               public int     office_id               { get; set; }
        [JsonPropertyName("appellate_officer_email")] public string? appellate_officer_email { get; set; }
        [JsonPropertyName("applicant_name")]          public string? applicant_name          { get; set; }
        [JsonPropertyName("applicant_mobile")]        public string? applicant_mobile        { get; set; }
        [JsonPropertyName("present_status")]          public string  present_status          { get; set; } = "PENDING";
        [JsonPropertyName("applied_date")]            public string? applied_date            { get; set; }
        [JsonPropertyName("last_updated_date")]       public string? last_updated_date       { get; set; }
        [JsonPropertyName("period_month")]            public int     period_month            { get; set; }
        [JsonPropertyName("period_year")]             public int     period_year             { get; set; }
    }
}
