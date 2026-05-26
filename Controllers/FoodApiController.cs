// =============================================================================
// FILE:      FoodApiController.cs
// DEPT:      FSD (Food & Supplies Department, West Bengal)
// AUTH:      HMAC  (X-API-KEY + X-TIMESTAMP + X-HMAC-SIGNATURE)
// FLOW:      NAME-BASED  — office_name / service_name / officer_email mapped
//                          via BcwMappingOrchestrator (UsesNameMapping = true)
// BASE PATH: /food
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MockExternalApi.Controllers
{
    [ApiController]
    [Route("food")]
    public sealed class FoodApiController : ControllerBase
    {
        private const string DeptCode = "FSD";
        private const string StateName = "West Bengal";
        private const int ServiceCodeBase = 31000;

        // ── Auth constants (read from config) ────────────────────────────────
        private readonly string _apiKey;
        private readonly string _hmacSecret;
        private readonly int    _toleranceMinutes;
        private readonly ILogger<FoodApiController> _logger;

        public FoodApiController(IConfiguration config, ILogger<FoodApiController> logger)
        {
            _apiKey           = config["FoodApi:ApiKey"]               ?? "food-api-key-001";
            _hmacSecret       = config["FoodApi:HmacSecret"]           ?? "food-hmac-secret-001";
            _toleranceMinutes = config.GetValue("FoodApi:TimestampToleranceMinutes", 5);
            _logger           = logger;
        }

        // ── Static data ───────────────────────────────────────────────────────

        private static readonly List<FoodOfficeDto> _offices = new()
        {
            new() { office_name = "Food & Supplies State Office Kolkata",        department_name = "FSD", office_level = "STATE",    district_name = "Kolkata",        is_active = true },
            new() { office_name = "Food & Supplies District Office Kolkata",     department_name = "FSD", office_level = "DISTRICT", district_name = "Kolkata",        is_active = true },
            new() { office_name = "Food & Supplies District Office Howrah",      department_name = "FSD", office_level = "DISTRICT", district_name = "Howrah",         is_active = true },
            new() { office_name = "Food & Supplies District Office Nadia",       department_name = "FSD", office_level = "DISTRICT", district_name = "Nadia",          is_active = true },
            new() { office_name = "Food & Supplies District Office Murshidabad", department_name = "FSD", office_level = "DISTRICT", district_name = "Murshidabad",    is_active = true },
            new() { office_name = "Food & Supplies District Office Bardhaman",   department_name = "FSD", office_level = "DISTRICT", district_name = "Purba Bardhaman",is_active = true },
            new() { office_name = "Food & Supplies Block Office Bankura",        department_name = "FSD", office_level = "BLOCK",    district_name = "Bankura",        is_active = true },
            new() { office_name = "Food & Supplies Block Office Malda",          department_name = "FSD", office_level = "BLOCK",    district_name = "Malda",          is_active = true },
        };

        private static readonly List<FoodServiceDto> _services = new()
        {
            new() { service_name = "Ration Card Issuance",     department_name = "FSD", stipulated_days = 15, stipulated_text = "15 working days", resolution_days = 20, appeal_days = 30, reappeal_days = 60, is_active = true },
            new() { service_name = "Ration Card Modification",  department_name = "FSD", stipulated_days = 10, stipulated_text = "10 working days", resolution_days = 15, appeal_days = 30, reappeal_days = 60, is_active = true },
            new() { service_name = "Fair Price Shop Licence",   department_name = "FSD", stipulated_days = 30, stipulated_text = "30 working days", resolution_days = 35, appeal_days = 30, reappeal_days = 60, is_active = true },
            new() { service_name = "Ration Card Surrender",     department_name = "FSD", stipulated_days = 7,  stipulated_text = "7 working days",  resolution_days = 10, appeal_days = 30, reappeal_days = 60, is_active = true },
            new() { service_name = "Duplicate Ration Card",     department_name = "FSD", stipulated_days = 12, stipulated_text = "12 working days", resolution_days = 17, appeal_days = 30, reappeal_days = 60, is_active = true },
            new() { service_name = "Ration Card Name Addition",  department_name = "FSD", stipulated_days = 10, stipulated_text = "10 working days", resolution_days = 15, appeal_days = 30, reappeal_days = 60, is_active = true },
            new() { service_name = "Ration Card Name Deletion",  department_name = "FSD", stipulated_days = 10, stipulated_text = "10 working days", resolution_days = 15, appeal_days = 30, reappeal_days = 60, is_active = true },
            new() { service_name = "Fair Price Shop Renewal",   department_name = "FSD", stipulated_days = 21, stipulated_text = "21 working days", resolution_days = 26, appeal_days = 30, reappeal_days = 60, is_active = true },
        };

        private static readonly List<FoodUserDto> _users = new()
        {
            new() { officer_email = "do.fsd.kol@wb.gov.in",  full_name = "Sanjib Chatterjee",  mobile_no = "9830200001", designation = "District Supply Officer",  role_key = "DESIGNATED_OFFICER", office_name = "Food & Supplies District Office Kolkata",     department_name = "FSD", is_active = true },
            new() { officer_email = "ao.fsd.hq@wb.gov.in",   full_name = "Ruma Dutta",          mobile_no = "9830200002", designation = "Assistant Director",       role_key = "APPELLATE_OFFICER",  office_name = "Food & Supplies State Office Kolkata",       department_name = "FSD", is_active = true },
            new() { officer_email = "ro.fsd.hwh@wb.gov.in",  full_name = "Prosenjit Nandi",     mobile_no = "9830200003", designation = "Regional Supply Officer",  role_key = "REVIEWING_OFFICER",  office_name = "Food & Supplies District Office Howrah",     department_name = "FSD", is_active = true },
            new() { officer_email = "do.fsd.nda@wb.gov.in",  full_name = "Ananya Mukherjee",    mobile_no = "9830200004", designation = "District Supply Officer",  role_key = "DESIGNATED_OFFICER", office_name = "Food & Supplies District Office Nadia",      department_name = "FSD", is_active = true },
            new() { officer_email = "ao.fsd.mur@wb.gov.in",  full_name = "Tapas Kundu",         mobile_no = "9830200005", designation = "Assistant Supply Officer", role_key = "APPELLATE_OFFICER",  office_name = "Food & Supplies District Office Murshidabad",department_name = "FSD", is_active = true },
            new() { officer_email = "do.fsd.brd@wb.gov.in",  full_name = "Supriya Ghosh",       mobile_no = "9830200006", designation = "District Supply Officer",  role_key = "DESIGNATED_OFFICER", office_name = "Food & Supplies District Office Bardhaman",  department_name = "FSD", is_active = true },
            new() { officer_email = "ro.fsd.bnk@wb.gov.in",  full_name = "Debashis Roy",        mobile_no = "9830200007", designation = "Block Supply Officer",     role_key = "REVIEWING_OFFICER",  office_name = "Food & Supplies Block Office Bankura",       department_name = "FSD", is_active = true },
            new() { officer_email = "ro.fsd.mld@wb.gov.in",  full_name = "Parna Sen",           mobile_no = "9830200008", designation = "Block Supply Officer",     role_key = "REVIEWING_OFFICER",  office_name = "Food & Supplies Block Office Malda",         department_name = "FSD", is_active = true },
        };

        private static readonly List<FoodAckDto> _acks = new()
        {
            new() { acknowledgement_no = "FSD/2026/20001", application_no = "APP/FSD/2026/201", service_name = "Ration Card Issuance",     department_name = "FSD", office_name = "Food & Supplies District Office Kolkata",     officer_email = "do.fsd.kol@wb.gov.in", applicant_name = "Nilufar Begum",    present_status = "RESOLVED",    applied_date = "2026-05-01", last_updated_date = "2026-05-16" },
            new() { acknowledgement_no = "FSD/2026/20002", application_no = "APP/FSD/2026/202", service_name = "Ration Card Modification",  department_name = "FSD", office_name = "Food & Supplies State Office Kolkata",       officer_email = "ao.fsd.hq@wb.gov.in",  applicant_name = "Kartik Ghosh",     present_status = "RESOLVED",    applied_date = "2026-05-03", last_updated_date = "2026-05-18" },
            new() { acknowledgement_no = "FSD/2026/20003", application_no = "APP/FSD/2026/203", service_name = "Fair Price Shop Licence",   department_name = "FSD", office_name = "Food & Supplies District Office Howrah",     officer_email = "ro.fsd.hwh@wb.gov.in", applicant_name = "Swapna Mandal",    present_status = "RESOLVED",    applied_date = "2026-04-15", last_updated_date = "2026-05-18" },
            new() { acknowledgement_no = "FSD/2026/20004", application_no = "APP/FSD/2026/204", service_name = "Ration Card Surrender",     department_name = "FSD", office_name = "Food & Supplies District Office Nadia",      officer_email = "do.fsd.nda@wb.gov.in", applicant_name = "Suresh Biswas",    present_status = "PENDING",     applied_date = "2026-05-05", last_updated_date = "2026-05-05" },
            new() { acknowledgement_no = "FSD/2026/20005", application_no = "APP/FSD/2026/205", service_name = "Duplicate Ration Card",     department_name = "FSD", office_name = "Food & Supplies District Office Murshidabad", officer_email = "ao.fsd.mur@wb.gov.in", applicant_name = "Priya Chatterjee", present_status = "RESOLVED",    applied_date = "2026-05-08", last_updated_date = "2026-05-23" },
            new() { acknowledgement_no = "FSD/2026/20006", application_no = "APP/FSD/2026/206", service_name = "Ration Card Name Addition",  department_name = "FSD", office_name = "Food & Supplies District Office Bardhaman",  officer_email = "do.fsd.brd@wb.gov.in", applicant_name = "Manoj Sharma",     present_status = "RESOLVED",    applied_date = "2026-05-10", last_updated_date = "2026-05-25" },
            new() { acknowledgement_no = "FSD/2026/20007", application_no = "APP/FSD/2026/207", service_name = "Ration Card Name Deletion",  department_name = "FSD", office_name = "Food & Supplies Block Office Bankura",       officer_email = "ro.fsd.bnk@wb.gov.in", applicant_name = "Puja Ghosh",       present_status = "RESOLVED",    applied_date = "2026-04-20", last_updated_date = "2026-05-20" },
            new() { acknowledgement_no = "FSD/2026/20008", application_no = "APP/FSD/2026/208", service_name = "Fair Price Shop Renewal",   department_name = "FSD", office_name = "Food & Supplies Block Office Malda",         officer_email = "ro.fsd.mld@wb.gov.in", applicant_name = "Tapas Biswas",     present_status = "IN_PROGRESS", applied_date = "2026-05-12", last_updated_date = "2026-05-19" },
            new() { acknowledgement_no = "FSD/2026/20009", application_no = "APP/FSD/2026/209", service_name = "Ration Card Issuance",     department_name = "FSD", office_name = "Food & Supplies District Office Kolkata",     officer_email = "do.fsd.kol@wb.gov.in", applicant_name = "Rekha Haldar",     present_status = "PENDING",     applied_date = "2026-05-14", last_updated_date = "2026-05-14" },
            new() { acknowledgement_no = "FSD/2026/20010", application_no = "APP/FSD/2026/210", service_name = "Ration Card Modification",  department_name = "FSD", office_name = "Food & Supplies District Office Howrah",     officer_email = "ro.fsd.hwh@wb.gov.in", applicant_name = "Arun Pal",         present_status = "RESOLVED",    applied_date = "2026-04-28", last_updated_date = "2026-05-22" },
        };

        // ── Endpoints ─────────────────────────────────────────────────────────

        [HttpGet("offices")]
        public IActionResult GetOffices([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var auth = ValidateHmac();
            if (auth != null) return auth;

            var projectedAll = _offices
                .Select(o => new
                {
                    office_code = ResolveOfficeCode(o.office_name),
                    office_name = o.office_name,
                    district_name = o.district_name,
                    state_name = StateName,
                    department_code = DeptCode,
                    is_active = o.is_active
                })
                .ToList();

            var (paged, total) = Paginate(projectedAll, page, page_size);
            _logger.LogInformation("[FOOD] GET /food/offices → {Count}/{Total}", paged.Count, total);
            return Ok(Wrap(paged, total, "FOOD-OFFICE"));
        }

        [HttpGet("services")]
        public IActionResult GetServices([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var auth = ValidateHmac();
            if (auth != null) return auth;

            var projectedAll = _services
                .Select(s => new
                {
                    service_code = ResolveServiceCode(s.service_name),
                    service_name = s.service_name,
                    department_code = DeptCode,
                    stipulated_days = s.stipulated_days,
                    stipulated_text = s.stipulated_text,
                    resolution_days = s.resolution_days,
                    appeal_days = s.appeal_days,
                    reappeal_days = s.reappeal_days,
                    is_active = s.is_active
                })
                .ToList();

            var (paged, total) = Paginate(projectedAll, page, page_size);
            _logger.LogInformation("[FOOD] GET /food/services → {Count}/{Total}", paged.Count, total);
            return Ok(Wrap(paged, total, "FOOD-SERVICE"));
        }

        [HttpGet("users")]
        [HttpGet("officers")]
        public IActionResult GetUsers([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var auth = ValidateHmac();
            if (auth != null) return auth;

            var projectedAll = _users
                .Select(u => new
                {
                    official_email = u.officer_email,
                    full_name = u.full_name,
                    mobile_no = u.mobile_no,
                    designation = u.designation,
                    office_code = ResolveOfficeCode(u.office_name),
                    department_code = DeptCode,
                    role_key = u.role_key,
                    is_active = u.is_active
                })
                .ToList();

            var (paged, total) = Paginate(projectedAll, page, page_size);
            _logger.LogInformation("[FOOD] GET /food/users → {Count}/{Total}", paged.Count, total);
            return Ok(Wrap(paged, total, "FOOD-USER"));
        }

        [HttpGet("acknowledgements")]
        public IActionResult GetAcknowledgements([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var auth = ValidateHmac();
            if (auth != null) return auth;

            var projectedAll = _acks
                .Select(a => new
                {
                    acknowledgement_no = a.acknowledgement_no,
                    application_number = a.application_no,
                    applicant_name = a.applicant_name,
                    service_code = ResolveServiceCode(a.service_name),
                    office_code = ResolveOfficeCode(a.office_name),
                    official_email = a.officer_email,
                    present_status = NormalizeStatus(a.present_status),
                    applied_date = a.applied_date,
                    last_updated_date = a.last_updated_date
                })
                .ToList();

            var (paged, total) = Paginate(projectedAll, page, page_size);
            _logger.LogInformation("[FOOD] GET /food/acknowledgements → {Count}/{Total}", paged.Count, total);
            return Ok(Wrap(paged, total, "FOOD-ACK"));
        }

        // ── HMAC validation ───────────────────────────────────────────────────

        private IActionResult? ValidateHmac()
        {
            var path = Request.Path + Request.QueryString;

            if (!Request.Headers.TryGetValue("X-API-KEY", out var apiKey) || apiKey != _apiKey)
                return Unauthorized(new { error = "FOOD: Invalid or missing X-API-KEY" });

            if (!Request.Headers.TryGetValue("X-TIMESTAMP", out var timestamp) || string.IsNullOrWhiteSpace(timestamp))
                return Unauthorized(new { error = "FOOD: X-TIMESTAMP missing" });

            if (DateTime.TryParse(timestamp, null, System.Globalization.DateTimeStyles.RoundtripKind, out var ts))
                if (Math.Abs((DateTime.UtcNow - ts.ToUniversalTime()).TotalMinutes) > _toleranceMinutes)
                    return StatusCode(403, new { error = "FOOD: Timestamp expired" });

            if (!Request.Headers.TryGetValue("X-HMAC-SIGNATURE", out var incoming) || string.IsNullOrWhiteSpace(incoming))
                return Unauthorized(new { error = "FOOD: X-HMAC-SIGNATURE missing" });

            var raw      = $"GET|{path}|{timestamp}|";
            var keyBytes = Encoding.UTF8.GetBytes(_hmacSecret);
            var msgBytes = Encoding.UTF8.GetBytes(raw);
            using var hmac = new HMACSHA256(keyBytes);
            var expected = Convert.ToBase64String(hmac.ComputeHash(msgBytes));

            _logger.LogInformation("[FOOD-HMAC] raw={Raw} expected={Exp}", raw, expected);

            if (!CryptographicOperations.FixedTimeEquals(
                    Encoding.UTF8.GetBytes(expected),
                    Encoding.UTF8.GetBytes(incoming.ToString())))
                return Unauthorized(new { error = "FOOD: Signature mismatch" });

            return null;
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static (List<T> Page, int Total) Paginate<T>(List<T> all, int page, int pageSize)
        {
            pageSize = Math.Clamp(pageSize, 1, 500);
            page     = Math.Max(1, page);
            return (all.Skip((page - 1) * pageSize).Take(pageSize).ToList(), all.Count);
        }

        private static string ResolveOfficeCode(string officeName)
        {
            if (string.IsNullOrWhiteSpace(officeName)) return $"{DeptCode}-OFF-000";
            var index = _offices.FindIndex(o => string.Equals(o.office_name, officeName, StringComparison.OrdinalIgnoreCase));
            return index >= 0 ? $"{DeptCode}-OFF-{index + 1:000}" : $"{DeptCode}-OFF-999";
        }

        private static int ResolveServiceCode(string serviceName)
        {
            if (string.IsNullOrWhiteSpace(serviceName)) return 0;
            var index = _services.FindIndex(s => string.Equals(s.service_name, serviceName, StringComparison.OrdinalIgnoreCase));
            return index >= 0 ? ServiceCodeBase + index + 1 : ServiceCodeBase + 999;
        }

        private static string NormalizeStatus(string? status)
            => status?.Trim().ToUpperInvariant() switch
            {
                "RESOLVED" => "disposed",
                "PENDING" => "pending",
                "IN_PROGRESS" => "in_progress",
                "INPROGRESS" => "in_progress",
                _ => (status ?? "pending").ToLowerInvariant()
            };

        private static object Wrap<T>(List<T> data, int total, string pipeline) => new
        {
            success    = true,
            message    = "OK",
            payload_id = $"{pipeline}-{DateTime.UtcNow:yyyyMMddHH}",
            totalCount = total,
            data
        };
    }

    // ── DTOs ──────────────────────────────────────────────────────────────────

    public sealed class FoodOfficeDto
    {
        [JsonPropertyName("office_name")]        public string  office_name      { get; set; } = default!;
        [JsonPropertyName("department_name")]    public string  department_name  { get; set; } = default!;
        [JsonPropertyName("office_level")]       public string  office_level     { get; set; } = default!;
        [JsonPropertyName("district_name")]      public string? district_name    { get; set; }
        [JsonPropertyName("block_name")]         public string? block_name       { get; set; }
        [JsonPropertyName("municipality_name")]  public string? municipality_name{ get; set; }
        [JsonPropertyName("parent_office_name")] public string? parent_office_name{ get; set; }
        [JsonPropertyName("is_active")]          public bool    is_active        { get; set; }
    }

    public sealed class FoodServiceDto
    {
        [JsonPropertyName("service_name")]     public string  service_name     { get; set; } = default!;
        [JsonPropertyName("department_name")]  public string  department_name  { get; set; } = default!;
        [JsonPropertyName("stipulated_days")]  public int?    stipulated_days  { get; set; }
        [JsonPropertyName("stipulated_text")]  public string? stipulated_text  { get; set; }
        [JsonPropertyName("resolution_days")]  public int?    resolution_days  { get; set; }
        [JsonPropertyName("appeal_days")]      public int?    appeal_days      { get; set; }
        [JsonPropertyName("reappeal_days")]    public int?    reappeal_days    { get; set; }
        [JsonPropertyName("is_active")]        public bool    is_active        { get; set; }
    }

    public sealed class FoodUserDto
    {
        [JsonPropertyName("officer_email")]    public string  officer_email    { get; set; } = default!;
        [JsonPropertyName("full_name")]        public string  full_name        { get; set; } = default!;
        [JsonPropertyName("mobile_no")]        public string? mobile_no        { get; set; }
        [JsonPropertyName("designation")]      public string  designation      { get; set; } = default!;
        [JsonPropertyName("role_key")]         public string  role_key         { get; set; } = default!;
        [JsonPropertyName("office_name")]      public string  office_name      { get; set; } = default!;
        [JsonPropertyName("department_name")]  public string  department_name  { get; set; } = default!;
        [JsonPropertyName("is_active")]        public bool    is_active        { get; set; }
    }

    public sealed class FoodAckDto
    {
        [JsonPropertyName("acknowledgement_no")] public string  acknowledgement_no { get; set; } = default!;
        [JsonPropertyName("application_no")]     public string? application_no     { get; set; }
        [JsonPropertyName("service_name")]       public string  service_name       { get; set; } = default!;
        [JsonPropertyName("department_name")]    public string  department_name    { get; set; } = default!;
        [JsonPropertyName("office_name")]        public string  office_name        { get; set; } = default!;
        [JsonPropertyName("officer_email")]      public string? officer_email      { get; set; }
        [JsonPropertyName("applicant_name")]     public string  applicant_name     { get; set; } = default!;
        [JsonPropertyName("present_status")]     public string  present_status     { get; set; } = default!;
        [JsonPropertyName("applied_date")]       public string? applied_date       { get; set; }
        [JsonPropertyName("last_updated_date")]  public string? last_updated_date  { get; set; }
    }
}
