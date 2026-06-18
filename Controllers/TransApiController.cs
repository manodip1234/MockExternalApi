// =============================================================================
// FILE:      TransApiController.cs
// DEPT:      HFW (Health & Family Welfare Department, West Bengal)
// AUTH:      HMAC  (X-API-KEY + X-TIMESTAMP + X-HMAC-SIGNATURE + X-API-VERSION)
// FLOW:      NAME-BASED  — RTPS resolves codes from names (uses_name_mapping=true)
// BASE PATH: /hfw
// =============================================================================

using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace MockExternalApi.Controllers;

[ApiController]
[Route("hfw")]
public sealed class TransApiController : ControllerBase
{
    private const string DeptCode        = "HFW";
    private const string StateName       = "West Bengal";
    private const string TimestampFormat = "yyyy-MM-ddTHH:mm:ssZ";

    private readonly string _apiKey;
    private readonly string _hmacSecret;
    private readonly int    _toleranceMinutes;
    private readonly ILogger<TransApiController> _logger;

    public TransApiController(IConfiguration config, ILogger<TransApiController> logger)
    {
        _apiKey           = config["TransApi:ApiKey"]     ?? "hfw-api-key-001";
        _hmacSecret       = config["TransApi:HmacSecret"] ?? "hfw-hmac-secret-001";
        _toleranceMinutes = config.GetValue("TransApi:TimestampToleranceMinutes", 5);
        _logger           = logger;
    }

    private static readonly List<OfficeDto> Offices =
    [
        new() { office_name = "Health & Family Welfare State Office West Bengal", department_name = "Health & Family Welfare Department", office_level = "STATE", district_name = "Kolkata", parent_office_name = null, jurisdiction_mode = "LGD", jurisdiction_scope = "OFFICE", jurisdiction_groups = [ new() { group_name = "State Wide", jurisdiction = [ new() { state_name = "West Bengal" } ] } ], is_active = true },
        new() { office_name = "Health & Family Welfare District Office Nadia", department_name = "Health & Family Welfare Department", office_level = "DISTRICT", district_name = "Nadia", parent_office_name = "Health & Family Welfare State Office West Bengal", jurisdiction_mode = "LGD", jurisdiction_scope = "OFFICE", jurisdiction_groups = [ new() { group_name = "District Wide", jurisdiction = [ new() { state_name = "West Bengal", district_name = "Nadia" } ] } ], is_active = true },
        new() { office_name = "Health & Family Welfare District Office North 24 Parganas", department_name = "Health & Family Welfare Department", office_level = "DISTRICT", district_name = "24 PARAGANAS NORTH", parent_office_name = "Health & Family Welfare State Office West Bengal", jurisdiction_mode = "LGD", jurisdiction_scope = "OFFICE", jurisdiction_groups = [ new() { group_name = "District Wide", jurisdiction = [ new() { state_name = "West Bengal", district_name = "24 PARAGANAS NORTH" } ] } ], is_active = true },
    ];

    private static readonly List<ServiceDto> Services =
    [
        new() { service_name = "Issuance CE License", department_name = "Health & Family Welfare Department", stipulated_days = 20, stipulated_hours = 0, stipulated_text = "Standard processing", resolution_days = 20, resolution_hours = 0, appeal_days = 15, appeal_hours = 0, reappeal_days = 30, reappeal_hours = 0, jurisdiction_mode = "LGD", jurisdiction_scope = "SERVICE", jurisdiction_groups = null, is_active = true, has_external_dependency = true },
        new() { service_name = "Issuance of PCPNDT License", department_name = "Health & Family Welfare Department", stipulated_days = 7, stipulated_hours = 0, stipulated_text = "Custom service coverage based on explicit jurisdiction rules", resolution_days = 10, resolution_hours = 0, appeal_days = 15, appeal_hours = 0, reappeal_days = 30, reappeal_hours = 0, jurisdiction_mode = "CUSTOM", jurisdiction_scope = "SERVICE", jurisdiction_groups = [ new() { group_name = "Health District 1", jurisdiction = [ new() { district_name = "Kolkata", block_name = "Kolkata Sadar" } ] } ], is_active = true, has_external_dependency = true },
        new() { service_name = "Issuance of Drug license ", department_name = "Health & Family Welfare Department", stipulated_days = 14, stipulated_hours = 0, stipulated_text = "Grouped health coverage", resolution_days = 18, resolution_hours = 0, appeal_days = 15, appeal_hours = 0, reappeal_days = 30, reappeal_hours = 0, jurisdiction_mode = "CUSTOM", jurisdiction_scope = "SERVICE", jurisdiction_groups = [ new() { group_name = "Health District 1", jurisdiction = [ new() { district_name = "24 PARAGANAS NORTH", block_name = "Barasat I" }, new() { district_name = "Howrah", block_name = "Sankrail" } ] }, new() { group_name = "Health District 2", jurisdiction = [ new() { district_name = "24 PARAGANAS NORTH", block_name = "Barasat I" }, new() { district_name = "24 PARAGANAS NORTH", block_name = "Barasat II" } ] } ], is_active = true, has_external_dependency = true },
        new() { service_name = "Birth Certificate Issuance", department_name = "Health & Family Welfare Department", stipulated_days = 7, stipulated_hours = 0, stipulated_text = "Standard processing", resolution_days = 10, resolution_hours = 0, appeal_days = 30, appeal_hours = 0, reappeal_days = 60, reappeal_hours = 0, jurisdiction_mode = "LGD", jurisdiction_scope = "SERVICE", jurisdiction_groups = null, is_active = true, has_external_dependency = false },
        new() { service_name = "Death Certificate Issuance", department_name = "Health & Family Welfare Department", stipulated_days = 7, stipulated_hours = 0, stipulated_text = "Standard processing", resolution_days = 10, resolution_hours = 0, appeal_days = 30, appeal_hours = 0, reappeal_days = 60, reappeal_hours = 0, jurisdiction_mode = "LGD", jurisdiction_scope = "SERVICE", jurisdiction_groups = null, is_active = true, has_external_dependency = false },
    ];

    private static readonly List<OfficerDto> Officers =
    [
        new() { official_email = "ps.hfw@wb.gov.in", full_name = "Dr. Indranil Roy", mobile_no = "9700100001", designation = "Principal Secretary", role_key = "DESIGNATED_OFFICER", office_name = "Health & Family Welfare State Office West Bengal", department_name = "Health & Family Welfare Department", is_active = true },
        new() { official_email = "cmoh.nadia.hfw@wb.gov.in", full_name = "Dr. Partha Sarathi Ghosh", mobile_no = "9700100003", designation = "Chief Medical Officer of Health", role_key = "DESIGNATED_OFFICER", office_name = "Health & Family Welfare District Office Nadia", department_name = "Health & Family Welfare Department", is_active = true },
        new() { official_email = "cmoh.n24.hfw@wb.gov.in", full_name = "Dr. Somnath Dutta", mobile_no = "9700100005", designation = "Chief Medical Officer of Health", role_key = "DESIGNATED_OFFICER", office_name = "Health & Family Welfare District Office North 24 Parganas", department_name = "Health & Family Welfare Department", is_active = true },
    ];

    private static readonly List<AckDto> Acknowledgements =
    [
        new() { acknowledgement_no = "HFW/2026/30001", application_no = "APP/HFW/2026/301", service_name = "Birth Certificate Issuance", office_name = "Health & Family Welfare District Office Nadia", official_email = "cmoh.nadia.hfw@wb.gov.in", department_name = "Health & Family Welfare Department", applicant_name = "Bapi Biswas", applicant_mobile = "9700500001", applicant_email = "bapi.b@example.com", present_status = "RESOLVED", applied_date = "2026-04-03", last_updated_date = "2026-04-09", NumberOfDaysBeyondDepartmentScope = 3 },
        new() { acknowledgement_no = "HFW/2026/30002", application_no = "APP/HFW/2026/302", service_name = "Death Certificate Issuance", office_name = "Health & Family Welfare District Office North 24 Parganas", official_email = "cmoh.n24.hfw@wb.gov.in", department_name = "Health & Family Welfare Department", applicant_name = "Rekha Sarkar", applicant_mobile = "9700500002", applicant_email = "rekha.s@example.com", present_status = "IN_PROGRESS", applied_date = "2026-04-07", last_updated_date = "2026-04-13", NumberOfDaysBeyondDepartmentScope = null },
        new() { acknowledgement_no = "HFW/2026/30003", application_no = "APP/HFW/2026/303", service_name = "Birth Certificate Issuance", office_name = "Health & Family Welfare State Office West Bengal", official_email = "ps.hfw@wb.gov.in", department_name = "Health & Family Welfare Department", applicant_name = "Purnima Barman", applicant_mobile = "9700500003", applicant_email = "purnima.b@example.com", present_status = "PENDING", applied_date = "2026-04-15", last_updated_date = "2026-04-20", NumberOfDaysBeyondDepartmentScope = 1 },
    ];

    [HttpGet("offices")]
    public IActionResult GetOffices([FromQuery] int page = 1, [FromQuery] int page_size = 100)
    {
        var auth = ValidateHmac();
        if (auth != null) return auth;

        var (paged, total) = Paginate(Offices, page, page_size);
        return Ok(ApiEnvelope<OfficeDto>.Ok("Office data", PayloadId("OFFICE"), total, paged));
    }

    [HttpGet("services")]
    public IActionResult GetServices([FromQuery] int page = 1, [FromQuery] int page_size = 100)
    {
        var auth = ValidateHmac();
        if (auth != null) return auth;

        var (paged, total) = Paginate(Services, page, page_size);
        return Ok(ApiEnvelope<ServiceDto>.Ok("Service data", PayloadId("SERVICE"), total, paged));
    }

    [HttpGet("users")]
    [HttpGet("officers")]
    public IActionResult GetOfficers([FromQuery] int page = 1, [FromQuery] int page_size = 100)
    {
        var auth = ValidateHmac();
        if (auth != null) return auth;

        var projected = Officers.Select(o => new
        {
            official_email = o.official_email,
            full_name = o.full_name,
            mobile_no = o.mobile_no,
            designation = o.designation,
            role_key = o.role_key,
            office_name = o.office_name,
            department_name = o.department_name,
            is_active = o.is_active
        }).Select(x => (object)x).ToList();

        var (paged, total) = Paginate(projected, page, page_size);
        return Ok(ApiEnvelope<object>.Ok("User data", PayloadId("USER"), total, paged));
    }

    [HttpGet("acknowledgements")]
    public IActionResult GetAcknowledgements([FromQuery] int page = 1, [FromQuery] int page_size = 100)
    {
        var auth = ValidateHmac();
        if (auth != null) return auth;

        var projected = Acknowledgements.Select(a => new
        {
            acknowledgement_no = a.acknowledgement_no,
            application_no = a.application_no,
            applicant_name = a.applicant_name,
            applicant_mobile = a.applicant_mobile,
            applicant_email = a.applicant_email,
            service_name = a.service_name,
            office_name = a.office_name,
            official_email = a.official_email,
            department_name = a.department_name,
            present_status = NormalizeStatus(a.present_status),
            applied_date = a.applied_date,
            last_updated_date = a.last_updated_date,
            number_of_days_beyond_department_scope = a.NumberOfDaysBeyondDepartmentScope
        }).Select(x => (object)x).ToList();

        var (paged, total) = Paginate(projected, page, page_size);
        return Ok(ApiEnvelope<object>.Ok("Acknowledgement data", PayloadId("ACK"), total, paged));
    }

    private IActionResult? ValidateHmac()
    {
        var pathAndQuery = Request.Path + Request.QueryString;

        if (!Request.Headers.TryGetValue("X-API-KEY", out var apiKey) || apiKey != _apiKey)
            return Unauthorized(new { error = "HFW: Invalid or missing X-API-KEY" });

        if (!Request.Headers.TryGetValue("X-TIMESTAMP", out var timestamp) || string.IsNullOrWhiteSpace(timestamp))
            return Unauthorized(new { error = "HFW: X-TIMESTAMP missing" });

        if (!DateTime.TryParseExact(
                timestamp.ToString(),
                TimestampFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out var parsedTimestamp))
            return Unauthorized(new { error = "HFW: Invalid X-TIMESTAMP format (expected UTC 'Z')" });

        var skewMinutes = Math.Abs((DateTime.UtcNow - parsedTimestamp.ToUniversalTime()).TotalMinutes);
        if (skewMinutes > _toleranceMinutes)
            return StatusCode(403, new { error = "HFW: Timestamp expired" });

        if (!Request.Headers.TryGetValue("X-HMAC-SIGNATURE", out var incoming) || string.IsNullOrWhiteSpace(incoming))
            return Unauthorized(new { error = "HFW: X-HMAC-SIGNATURE missing" });

        if (!Request.Headers.TryGetValue("X-API-VERSION", out var apiVersion) || string.IsNullOrWhiteSpace(apiVersion))
            return Unauthorized(new { error = "HFW: X-API-VERSION missing" });

        var raw = $"GET|{pathAndQuery}|{timestamp}|";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_hmacSecret));
        var expected = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(raw)));

        if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(expected),
                Encoding.UTF8.GetBytes(incoming.ToString())))
            return Unauthorized(new { error = "HFW: Signature mismatch" });

        return null;
    }

    private static string NormalizeStatus(string? status)
        => status?.Trim().ToUpperInvariant() switch
        {
            "PENDING"       => "PENDING",
            "IN_PROGRESS"   => "IN_PROGRESS",
            "INPROGRESS"    => "IN_PROGRESS",
            "RESOLVED"      => "RESOLVED",
            "REJECTED"      => "REJECTED",
            "DISPOSED"      => "DISPOSED",
            "DISPOSED_LATE" => "DISPOSED",
            _               => "PENDING"
        };

    private static string PayloadId(string type)
        => $"P-{DeptCode}-{type}-{DateTime.UtcNow:yyyyMMddHH}";

    private static (List<T> Page, int Total) Paginate<T>(IReadOnlyList<T> all, int page, int pageSize)
    {
        pageSize = Math.Clamp(pageSize, 1, 500);
        page     = Math.Max(1, page);
        return (all.Skip((page - 1) * pageSize).Take(pageSize).ToList(), all.Count);
    }

    public sealed class OfficeDto
    {
        [JsonPropertyName("office_name")]        public string  office_name        { get; set; } = default!;
        [JsonPropertyName("department_name")]    public string  department_name    { get; set; } = default!;
        [JsonPropertyName("office_level")]       public string  office_level       { get; set; } = default!;
        [JsonPropertyName("district_name")]      public string? district_name      { get; set; }
        [JsonPropertyName("block_name")]         public string? block_name         { get; set; }
        [JsonPropertyName("municipality_name")]  public string? municipality_name  { get; set; }
        [JsonPropertyName("gram_panchayat_name")] public string? gram_panchayat_name { get; set; }
        [JsonPropertyName("ward_name")]          public string? ward_name          { get; set; }
        [JsonPropertyName("parent_office_name")] public string? parent_office_name { get; set; }
        [JsonPropertyName("is_active")]          public bool    is_active          { get; set; } = true;
        [JsonPropertyName("jurisdiction_mode")]  public string? jurisdiction_mode  { get; set; }
        [JsonPropertyName("jurisdiction_scope")] public string? jurisdiction_scope { get; set; }
        [JsonPropertyName("jurisdiction_groups")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public List<JurisdictionGroupDto>? jurisdiction_groups { get; set; }
    }

    public sealed class ServiceDto
    {
        [JsonPropertyName("service_name")]     public string  service_name     { get; set; } = default!;
        [JsonPropertyName("department_name")]  public string  department_name  { get; set; } = default!;
        [JsonPropertyName("stipulated_days")]  public int?    stipulated_days  { get; set; }
        [JsonPropertyName("stipulated_text")]  public string? stipulated_text  { get; set; }
        [JsonPropertyName("stipulated_hours")] public int?    stipulated_hours { get; set; }
        [JsonPropertyName("resolution_days")]  public int?    resolution_days  { get; set; }
        [JsonPropertyName("resolution_hours")] public int?    resolution_hours { get; set; }
        [JsonPropertyName("appeal_days")]      public int?    appeal_days      { get; set; }
        [JsonPropertyName("appeal_hours")]     public int?    appeal_hours     { get; set; }
        [JsonPropertyName("reappeal_days")]    public int?    reappeal_days    { get; set; }
        [JsonPropertyName("reappeal_hours")]   public int?    reappeal_hours   { get; set; }
        [JsonPropertyName("is_active")]        public bool    is_active        { get; set; } = true;
        [JsonPropertyName("jurisdiction_mode")]  public string? jurisdiction_mode  { get; set; }
        [JsonPropertyName("jurisdiction_scope")] public string? jurisdiction_scope { get; set; }
        [JsonPropertyName("jurisdiction_groups")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public List<JurisdictionGroupDto>? jurisdiction_groups { get; set; }
        [JsonPropertyName("beyond_department_days")]   [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int?  beyond_department_days  { get; set; }
        [JsonPropertyName("has_external_dependency")] public bool   has_external_dependency { get; set; } = false;
    }

    public sealed class OfficerDto
    {
        [JsonPropertyName("official_email")] public string official_email { get; set; } = default!;
        [JsonPropertyName("full_name")] public string full_name { get; set; } = default!;
        [JsonPropertyName("mobile_no")] public string? mobile_no { get; set; }
        [JsonPropertyName("designation")] public string? designation { get; set; }
        [JsonPropertyName("role_key")] public string? role_key { get; set; }
        [JsonPropertyName("office_name")] public string? office_name { get; set; }
        [JsonPropertyName("department_name")] public string? department_name { get; set; }
        [JsonPropertyName("is_active")] public bool is_active { get; set; } = true;
    }

    public sealed class AckDto
    {
        [JsonPropertyName("acknowledgement_no")] public string acknowledgement_no { get; set; } = default!;
        [JsonPropertyName("application_no")] public string? application_no { get; set; }
        [JsonPropertyName("service_name")] public string service_name { get; set; } = default!;
        [JsonPropertyName("office_name")] public string office_name { get; set; } = default!;
        [JsonPropertyName("official_email")] public string? official_email { get; set; }
        [JsonPropertyName("department_name")] public string department_name { get; set; } = default!;
        [JsonPropertyName("applicant_name")] public string? applicant_name { get; set; }
        [JsonPropertyName("applicant_mobile")] public string? applicant_mobile { get; set; }
        [JsonPropertyName("applicant_email")] public string? applicant_email { get; set; }
        [JsonPropertyName("present_status")] public string present_status { get; set; } = "PENDING";
        [JsonPropertyName("applied_date")] public string? applied_date { get; set; }
        [JsonPropertyName("last_updated_date")] public string? last_updated_date { get; set; }
        [JsonPropertyName("number_of_days_beyond_department_scope")] public int? NumberOfDaysBeyondDepartmentScope { get; set; }
    }

    private sealed class ApiEnvelope<T>
    {
        [JsonPropertyName("success")] public bool success { get; set; }
        [JsonPropertyName("message")] public string message { get; set; } = string.Empty;
        [JsonPropertyName("payload_id")] public string payload_id { get; set; } = string.Empty;
        [JsonPropertyName("totalCount")] public int totalCount { get; set; }
        [JsonPropertyName("checksum")] public string? checksum { get; set; }
        [JsonPropertyName("data")] public List<T> data { get; set; } = [];

        public static ApiEnvelope<T> Ok(string message, string payloadId, int total, List<T> data)
        {
            var without = new ApiEnvelope<T>
            {
                success = true,
                message = message,
                payload_id = payloadId,
                totalCount = total,
                checksum = null,
                data = data
            };

            var raw = JsonSerializer.Serialize(without, JsonHelpers.WithoutNulls);
            without.checksum = JsonHelpers.Base64Sha256(raw);
            return without;
        }
    }

    private static class JsonHelpers
    {
        public static readonly JsonSerializerOptions WithoutNulls = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = null,
            WriteIndented = false
        };

        public static string Base64Sha256(string rawJson)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(rawJson));
            return Convert.ToBase64String(hash);
        }
    }
}
