// =============================================================================
// FILE:      BcwApiController.cs
// DEPT:      BCW (Backward Classes Welfare, West Bengal)
// AUTH:      HMAC / API_KEY (X-API-KEY + optional HMAC headers)
// FLOW:      NAME-BASED — RTPS resolves codes from names (uses_name_mapping=true)
// BASE PATH: /bcw
// =============================================================================

using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace MockExternalApi.Controllers;

[ApiController]
[Route("bcw")]
public sealed class BcwApiController : ControllerBase
{
    private const string DeptCode        = "BCW";
    private const string StateName       = "West Bengal";
    private const string TimestampFormat = "yyyy-MM-ddTHH:mm:ssZ";

    private readonly string _apiKey;
    private readonly string _hmacSecret;
    private readonly int    _toleranceMinutes;
    private readonly ILogger<BcwApiController> _logger;

    public BcwApiController(IConfiguration config, ILogger<BcwApiController> logger)
    {
        _apiKey           = config["BcwApi:ApiKey"]     ?? "bcw-test-api-key-001";
        _hmacSecret       = config["BcwApi:HmacSecret"] ?? "bcw-test-api-key-001";
        _toleranceMinutes = config.GetValue("BcwApi:TimestampToleranceMinutes", 5);
        _logger           = logger;
    }

    private static readonly List<BcwOfficeDto> Offices =
    [
        new() {
            office_name = "BCW State Office West Bengal",
            department_name = "Backward Classes Welfare Department",
            office_level = "STATE",
            district_name = "Kolkata",
            parent_office_name = null,
            jurisdiction_mode = "LGD",
            is_active = true 
        },
        new() {
            office_name = "Bankura Sub-division Office",
            department_name = "Backward Classes Welfare Department",
            office_level = "DISTRICT",
            district_name = "Bankura",
            parent_office_name = "BCW State Office West Bengal",
            jurisdiction_mode = "LGD",
            is_active = true 
        },
        new() {
            office_name = "BCW District Office Murshidabad",
            department_name = "Backward Classes Welfare Department",
            office_level = "DISTRICT",
            district_name = "Murshidabad",
            parent_office_name = "BCW State Office West Bengal",
            jurisdiction_mode = "LGD",
            is_active = true 
        },
    ];

    private static readonly List<BcwServiceDto> Services =
    [
        new()
        {
            service_name = "OBC Certificate Issuance",
            department_name = "Backward Classes Welfare Department",
            service_type = "HYBRID",
            stipulated_days = 30,
            stipulated_text = "Standard processing",
            stipulated_hours = 0,
            resolution_days = 30,
            resolution_hours = 0,
            appeal_days = 30,
            appeal_hours = 0,
            reappeal_days = 60,
            reappeal_hours = 0,
            jurisdiction_mode = "LGD",
            jurisdiction_groups = [ new() { group_name = "State Wide", jurisdiction = [ new() { state_name = "West Bengal" } ] } ],
            has_external_dependency = true,
            is_active = true
        },
        new()
        {
            service_name = "Minority Welfare Scholarship",
            department_name = "Backward Classes Welfare Department",
            service_type = "HYBRID",
            stipulated_days = 21,
            stipulated_text = "Custom processing",
            stipulated_hours = 0,
            resolution_days = 21,
            resolution_hours = 0,
            appeal_days = 30,
            appeal_hours = 0,
            reappeal_days = 60,
            reappeal_hours = 0,
            jurisdiction_mode = "CUSTOM",
            jurisdiction_groups = [ new() { group_name = "Custom District Coverage", jurisdiction = [ new() { district_name = "Bankura" }, new() { district_name = "Murshidabad" } ] } ],
            has_external_dependency = false,
            is_active = true
        }
    ];

    private static readonly List<BcwOfficerDto> Officers =
    [
        new() {
            official_email = "acs.bcw@wb.gov.in",
            full_name = "Ayan Chakraborty",
            mobile_no = "9800000001",
            designation = "Additional Chief Secretary",
            role_key = "DESIGNATED_OFFICER",
            office_name = "BCW State Office West Bengal",
            department_name = "Backward Classes Welfare Department",
            is_active = true 
        },
        new() {
            official_email = "sdosadarbankura@gmail.com",
            full_name = "PROLAY MONDAL",
            mobile_no = "9800000002",
            designation = "Sub-divisional Officer",
            role_key = "DESIGNATED_OFFICER",
            office_name = "Bankura Sub-division Office",
            department_name = "Backward Classes Welfare Department",
            is_active = true 
        },
        new() {
            official_email = "dwo.murshidabad.bcw@wb.gov.in",
            full_name = "Farhana Ali",
            mobile_no = "9800000003",
            designation = "District Welfare Officer",
            role_key = "DESIGNATED_OFFICER",
            office_name = "BCW District Office Murshidabad",
            department_name = "Backward Classes Welfare Department",
            is_active = true 
        }
    ];

    private static readonly List<BcwAcknowledgementDto> Acknowledgements =
    [
        new() {
            acknowledgement_no = "BCW/2026/10001",
            application_no = "APP/BCW/2026/101",
            service_name = "Issuance of Caste Certificate (SC/ST/OBC)",
            office_name = "Bankura Sub-division Office",
            official_email = "sdosadarbankura@gmail.com",
            department_name = "Backward Classes Welfare Department",
            applicant_name = "Ratan Bauri",
            date_of_birth = "1989-05-11",
            applicant_mobile = "9400500001",
            applicant_email = "ratan.b@example.com",
            present_status = "RESOLVED",
            applied_date = "2026-08-20",
            last_updated_date = "2026-04-20",
            NumberOfDaysBeyondDepartmentScope = 15 
        },
        new() {
            acknowledgement_no = "BCW/2026/10002",
            application_no = "APP/BCW/2026/102",
            service_name = "Minority Welfare Scholarship",
            office_name = "BCW District Office Murshidabad",
            official_email = "dwo.murshidabad.bcw@wb.gov.in",
            department_name = "Backward Classes Welfare Department",
            applicant_name = "Farida Khatun",
            date_of_birth = "1994-01-19",
            applicant_mobile = "9400500002",
            applicant_email = "farida.k@example.com",
            present_status = "IN_PROGRESS",
            applied_date = "2026-08-20",
            last_updated_date = "2026-04-22",
            NumberOfDaysBeyondDepartmentScope = 35 
        },
        new() {
            acknowledgement_no = "BCW/2026/10003",
            application_no = "APP/BCW/2026/103",
            service_name = "OBC Certificate Issuance",
            office_name = "BCW State Office West Bengal",
            official_email = "acs.bcw@wb.gov.in",
            department_name = "Backward Classes Welfare Department",
            applicant_name = "Subhash Mondal",
            date_of_birth = "1980-08-02",
            applicant_mobile = "9400500003",
            applicant_email = "subhash.m@example.com",
            present_status = "PENDING",
            applied_date = "2026-08-20",
            last_updated_date = "2026-04-25",
            NumberOfDaysBeyondDepartmentScope = 15 
        },
    ];

    [HttpGet("offices")]
    public IActionResult GetOffices([FromQuery] int page = 1, [FromQuery] int page_size = 100)
    {
        var auth = ValidateAuth();
        if (auth != null) return auth;

        var (paged, total) = Paginate(Offices, page, page_size);
        _logger.LogInformation("[BCW] GET /bcw/offices -> {Count}/{Total}", paged.Count, total);
        return Ok(ApiEnvelope<BcwOfficeDto>.Ok("Office data", PayloadId("OFFICE"), total, paged));
    }

    [HttpGet("services")]
    public IActionResult GetServices([FromQuery] int page = 1, [FromQuery] int page_size = 100)
    {
        var auth = ValidateAuth();
        if (auth != null) return auth;

        var (paged, total) = Paginate(Services, page, page_size);
        _logger.LogInformation("[BCW] GET /bcw/services -> {Count}/{Total}", paged.Count, total);
        return Ok(ApiEnvelope<BcwServiceDto>.Ok("Service data", PayloadId("SERVICE"), total, paged));
    }

    [HttpGet("officers")]
    public IActionResult GetOfficers([FromQuery] int page = 1, [FromQuery] int page_size = 100)
    {
        var auth = ValidateAuth();
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
        _logger.LogInformation("[BCW] GET /bcw/officers -> {Count}/{Total}", paged.Count, total);
        return Ok(ApiEnvelope<object>.Ok("User data", PayloadId("USER"), total, paged));
    }

    [HttpGet("acknowledgements")]
    public IActionResult GetAcknowledgements([FromQuery] int page = 1, [FromQuery] int page_size = 100)
    {
        var auth = ValidateAuth();
        if (auth != null) return auth;

        var projected = Acknowledgements.Select(a => new
        {
            acknowledgement_no = a.acknowledgement_no,
            application_no = a.application_no,
            application_number = a.application_no ?? a.acknowledgement_no,
            applicant_name = a.applicant_name,
            applicant_mobile = a.applicant_mobile,
            applicant_email = a.applicant_email,
            service_name = a.service_name,
            service_code = 1,
            office_name = a.office_name,
            office_code = "BCW_OFFICE_01",
            officer_email = a.official_email,
            official_email = a.official_email,
            department_name = a.department_name,
            department_code = "BCW",
            present_status = NormalizeStatus(a.present_status),
            applied_date = a.applied_date,
            last_updated_date = a.last_updated_date,
            number_of_days_beyond_department_scope = a.NumberOfDaysBeyondDepartmentScope
        }).Select(x => (object)x).ToList();

        var (paged, total) = Paginate(projected, page, page_size);
        _logger.LogInformation("[BCW] GET /bcw/acknowledgements -> {Count}/{Total}", paged.Count, total);
        return Ok(ApiEnvelope<object>.Ok("Acknowledgement data", PayloadId("ACK"), total, paged));
    }

    [HttpGet("acknowledgement/verify")]
    [HttpGet("/wb/bcwd/wbrtps_ack/wbrtps_ack")]
    public IActionResult VerifyAcknowledgement(
        [FromQuery(Name = "acknowledgement_no")] string? acknowledgementNo,
        [FromQuery(Name = "application_no")] string? applicationNo,
        [FromQuery(Name = "applid")] string? applid,
        [FromQuery(Name = "date_of_birth")] string? dateOfBirth,
        [FromQuery(Name = "dob")] string? dob,
        [FromQuery(Name = "service_name")] string? serviceName)
    {
        var auth = ValidateAuth();
        if (auth != null) return auth;

        var targetNo = (acknowledgementNo ?? applicationNo ?? applid)?.Trim();
        var targetDob = (dateOfBirth ?? dob)?.Trim();

        if (string.IsNullOrWhiteSpace(targetNo))
        {
            return Ok(new
            {
                success = false,
                error_code = "VALIDATION_INVALID_FIELD_VALUE",
                error_message = "acknowledgement_no, application_no, or applid must be supplied"
            });
        }

        var match = Acknowledgements.FirstOrDefault(a =>
            string.Equals(a.acknowledgement_no, targetNo, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(a.application_no, targetNo, StringComparison.OrdinalIgnoreCase));

        // If not statically found, generate a dynamic mock record for flexible integration testing
        if (match == null)
        {
            _logger.LogInformation("[BCW] Verify: generating dynamic mock for {TargetNo}", targetNo);
            match = new BcwAcknowledgementDto
            {
                acknowledgement_no = targetNo,
                application_no = targetNo.StartsWith("APP/") ? targetNo : $"APP/{targetNo}",
                service_name = !string.IsNullOrWhiteSpace(serviceName) ? serviceName : "Issuance of Caste Certificate (SC/ST/OBC)",
                office_name = "Bankura Sub-division Office",
                official_email = "sdosadarbankura@gmail.com",
                department_name = "Backward Classes Welfare Department",
                applicant_name = "Sample Applicant",
                date_of_birth = !string.IsNullOrWhiteSpace(targetDob) ? targetDob : "1990-01-01",
                applicant_mobile = "9400500001",
                applicant_email = "applicant@example.com",
                present_status = "IN_PROGRESS",
                applied_date = DateTime.UtcNow.AddDays(-10).ToString("yyyy-MM-dd"),
                last_updated_date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                NumberOfDaysBeyondDepartmentScope = 0
            };
        }

        var verifyRecord = ToVerifyRecord(match);
        _logger.LogInformation("[BCW] GET acknowledgement/verify -> matched AckNo={AckNo}", match.acknowledgement_no);

        return Ok(new
        {
            success = true,
            message = "Acknowledgement verified",
            data = verifyRecord,
            status = new
            {
                success = true,
                error = "1",
                msg = "Acknowledgement verified",
                payload_id = PayloadId("ACK-VERIFY"),
                data = new[] { verifyRecord }
            }
        });
    }

    private IActionResult? ValidateAuth()
    {
        var pathAndQuery = Request.Path + Request.QueryString;
        var hasApiKey = Request.Headers.TryGetValue("X-API-KEY", out var apiKey);

        // If HMAC signature is present, validate HMAC
        if (Request.Headers.TryGetValue("X-HMAC-SIGNATURE", out var incoming) && !string.IsNullOrWhiteSpace(incoming))
        {
            if (!Request.Headers.TryGetValue("X-TIMESTAMP", out var timestamp) || string.IsNullOrWhiteSpace(timestamp))
                return Unauthorized(new { error = "BCW: X-TIMESTAMP missing" });

            if (!DateTime.TryParseExact(
                    timestamp.ToString(),
                    TimestampFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out var parsedTimestamp))
                return Unauthorized(new { error = "BCW: Invalid X-TIMESTAMP format (expected UTC 'Z')" });

            var skewMinutes = Math.Abs((DateTime.UtcNow - parsedTimestamp.ToUniversalTime()).TotalMinutes);
            if (skewMinutes > _toleranceMinutes)
                return StatusCode(403, new { error = "BCW: Timestamp expired" });

            var raw = $"GET|{pathAndQuery}|{timestamp}|";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_hmacSecret));
            var expected = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(raw)));

            if (!CryptographicOperations.FixedTimeEquals(
                    Encoding.UTF8.GetBytes(expected),
                    Encoding.UTF8.GetBytes(incoming.ToString())))
                return Unauthorized(new { error = "BCW: Signature mismatch" });
        }
        else if (hasApiKey)
        {
            if (apiKey != _apiKey && apiKey != "rtps-demo-key-001" && apiKey != "test-api-key")
            {
                _logger.LogWarning("[BCW] API Key provided but does not match default keys: {Key}", (object)apiKey.ToString());
            }
        }

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

    // DTOs
    public sealed class BcwOfficeDto
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
        [JsonPropertyName("jurisdiction_mode")]  public string? jurisdiction_mode  { get; set; }
        [JsonPropertyName("jurisdiction_scope")] public string? jurisdiction_scope { get; set; }
        [JsonPropertyName("jurisdiction_groups")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public List<JurisdictionGroupDto>? jurisdiction_groups { get; set; }
        [JsonPropertyName("is_active")]          public bool    is_active          { get; set; } = true;
    }

    public sealed class BcwServiceDto
    {
        [JsonPropertyName("service_name")]     public string  service_name     { get; set; } = default!;
        [JsonPropertyName("department_name")]  public string  department_name  { get; set; } = default!;
        [JsonPropertyName("service_type")]     public string?  service_type     { get; set; }
        [JsonPropertyName("stipulated_days")]  public int?    stipulated_days  { get; set; }
        [JsonPropertyName("stipulated_text")]  public string? stipulated_text  { get; set; }
        [JsonPropertyName("stipulated_hours")] public int?    stipulated_hours { get; set; }
        [JsonPropertyName("resolution_days")]  public int?    resolution_days  { get; set; }
        [JsonPropertyName("resolution_hours")] public int?    resolution_hours { get; set; }
        [JsonPropertyName("appeal_days")]      public int?    appeal_days      { get; set; }
        [JsonPropertyName("appeal_hours")]     public int?    appeal_hours     { get; set; }
        [JsonPropertyName("reappeal_days")]    public int?    reappeal_days    { get; set; }
        [JsonPropertyName("reappeal_hours")]   public int?    reappeal_hours   { get; set; }
        [JsonPropertyName("jurisdiction_mode")]  public string? jurisdiction_mode  { get; set; }
        [JsonPropertyName("jurisdiction_scope")] public string? jurisdiction_scope { get; set; }
        [JsonPropertyName("jurisdiction_groups")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public List<JurisdictionGroupDto>? jurisdiction_groups { get; set; }
        [JsonPropertyName("is_active")]        public bool    is_active               { get; set; } = true;
        [JsonPropertyName("has_external_dependency")] public bool   has_external_dependency { get; set; } = false;
    }

    public sealed class BcwOfficerDto
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

    public sealed class BcwAcknowledgementDto
    {
        [JsonPropertyName("acknowledgement_no")] public string acknowledgement_no { get; set; } = default!;
        [JsonPropertyName("application_no")] public string? application_no { get; set; }
        [JsonPropertyName("service_name")] public string service_name { get; set; } = default!;
        [JsonPropertyName("office_name")] public string office_name { get; set; } = default!;
        [JsonPropertyName("official_email")] public string? official_email { get; set; }
        [JsonPropertyName("department_name")] public string department_name { get; set; } = default!;
        [JsonPropertyName("applicant_name")] public string? applicant_name { get; set; }
        [JsonPropertyName("date_of_birth")] public string? date_of_birth { get; set; }
        [JsonPropertyName("applicant_mobile")] public string? applicant_mobile { get; set; }
        [JsonPropertyName("applicant_email")] public string? applicant_email { get; set; }
        [JsonPropertyName("present_status")] public string present_status { get; set; } = "PENDING";
        [JsonPropertyName("applied_date")] public string? applied_date { get; set; }
        [JsonPropertyName("last_updated_date")] public string? last_updated_date { get; set; }
        [JsonPropertyName("number_of_days_beyond_department_scope")] public int? NumberOfDaysBeyondDepartmentScope { get; set; }
    }

    private static object ToVerifyRecord(BcwAcknowledgementDto a) => new
    {
        acknowledgement_no = a.acknowledgement_no,
        application_no = a.application_no ?? a.acknowledgement_no,
        application_number = a.application_no ?? a.acknowledgement_no,
        applicant_name = a.applicant_name ?? "Sample Applicant",
        service_name = a.service_name,
        service_code = 1,
        office_name = a.office_name,
        office_code = a.office_name == "Bankura Sub-division Office" ? "OFF-DBFFD2BEC8AB" : "BCW_OFFICE_01",
        official_email = a.official_email ?? "sdosadarbankura@gmail.com",
        officer_email = a.official_email ?? "sdosadarbankura@gmail.com",
        user_id = a.official_email ?? "sdosadarbankura@gmail.com",
        department_name = a.department_name,
        department_code = "BCW",
        present_status = NormalizeStatus(a.present_status),
        applied_date = a.applied_date ?? DateTime.UtcNow.ToString("yyyy-MM-dd"),
        assign_date = a.applied_date ?? DateTime.UtcNow.ToString("yyyy-MM-dd"),
        assigned_date = a.applied_date ?? DateTime.UtcNow.ToString("yyyy-MM-dd"),
        pending_with_designation = a.office_name,
        officer_name = Officers.FirstOrDefault(o =>
            string.Equals(o.official_email, a.official_email, StringComparison.OrdinalIgnoreCase))?.full_name ?? "BCW Officer",
        number_of_days_beyond_department_scope = a.NumberOfDaysBeyondDepartmentScope ?? 0
    };

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
            var checksum = JsonHelpers.Base64Sha256(raw);

            without.checksum = checksum;
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
