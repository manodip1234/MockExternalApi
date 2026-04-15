// =============================================================================
// FILE:      BcwApiController.cs
// PROJECT:   MockExternalApi
// PURPOSE:   Simulates the BCW department external API (name-based, no codes).
//            All names/emails MUST match bcw_name_code_mapping.ext_name exactly.
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace MockExternalApi.Controllers
{
    [ApiController]
    [Route("")]
    public class BcwApiController : ControllerBase
    {
        private readonly string _authHeaderName;
        private readonly string _authCredential;
        private readonly ILogger<BcwApiController> _logger;

        public BcwApiController(IConfiguration config, ILogger<BcwApiController> logger)
        {
            _authHeaderName = config["BcwApi:AuthHeaderName"] ?? "X-API-KEY";
            _authCredential = config["BcwApi:AuthCredential"] ?? "bcw-test-api-key-001";
            _logger = logger;
        }

        // ─────────────────────────────────────────────────────────────
        // OFFICES
        // Names MUST match bcw_name_code_mapping.ext_name exactly
        // ─────────────────────────────────────────────────────────────
        private static readonly List<BcwOfficeDto> _offices =
        [
            new() { office_name = "BCW District Office Kolkata",       department_name = "BCW", office_level = "DISTRICT", district_name = "Kolkata",           is_active = true },
            new() { office_name = "BCW State Office Kolkata",           department_name = "BCW", office_level = "STATE",    district_name = "Kolkata",           is_active = true },
            new() { office_name = "BCW District Office Bardhaman",      department_name = "BCW", office_level = "DISTRICT", district_name = "Bardhaman",         is_active = true },
            new() { office_name = "BCW Block Office North 24 Parganas", department_name = "BCW", office_level = "BLOCK",    district_name = "North 24 Parganas", is_active = true },
        ];

        // ─────────────────────────────────────────────────────────────
        // SERVICES
        // Names MUST match bcw_name_code_mapping.ext_name exactly
        // ─────────────────────────────────────────────────────────────
        private static readonly List<BcwServiceDto> _services =
        [
            new() { service_name = "BCW Caste Certificate Service", department_name = "BCW", stipulated_days = 30, stipulated_text = "30 days", resolution_days = 30, appeal_days = 30, reappeal_days = 60, is_active = true },
            new() { service_name = "BCW Welfare Scheme Service",    department_name = "BCW", stipulated_days = 45, stipulated_text = "45 days", resolution_days = 45, appeal_days = 30, reappeal_days = 60, is_active = true },
            new() { service_name = "BCW Scholarship Grant Service", department_name = "BCW", stipulated_days = 20, stipulated_text = "20 days", resolution_days = 20, appeal_days = 30, reappeal_days = 60, is_active = true },
        ];

        // ─────────────────────────────────────────────────────────────
        // OFFICERS
        // Emails MUST match bcw_name_code_mapping.ext_name exactly
        // dotest@gmail.com → id=8 (DESIGNATED)
        // aotest@gmail.com → id=7 (APPELLATE)
        // rotest@gmail.com → id=6 (REVIEWING)
        // ─────────────────────────────────────────────────────────────
        private static readonly List<BcwOfficerDto> _officers =
        [
            new() { officer_email = "dotest@gmail.com", full_name = "DO BCW Officer", mobile_no = "9300100001", designation = "Additional Chief Secretary", role_key = "DESIGNATED_OFFICER", office_name = "BCW Block Office North 24 Parganas", department_name = "BCW", is_active = true },
            new() { officer_email = "aotest@gmail.com", full_name = "AO BCW Officer", mobile_no = "9300100002", designation = "Additional Chief Secretary", role_key = "APPELLATE_OFFICER",  office_name = "BCW District Office Bardhaman",      department_name = "BCW", is_active = true },
            new() { officer_email = "rotest@gmail.com", full_name = "RO BCW Officer", mobile_no = "9300100003", designation = "Additional Chief Secretary", role_key = "REVIEWING_OFFICER",  office_name = "BCW District Office Kolkata",        department_name = "BCW", is_active = true },
        ];

        // ─────────────────────────────────────────────────────────────
        // ACKNOWLEDGEMENTS
        // service_name, office_name, officer_email MUST all match
        // bcw_name_code_mapping.ext_name exactly
        // ─────────────────────────────────────────────────────────────
        private static readonly List<BcwAcknowledgementDto> _acknowledgements =
        [
            new() { acknowledgement_no = "BCW/2026/00001", application_no = "APP/2026/001", service_name = "BCW Caste Certificate Service",  department_name = "BCW", office_name = "BCW District Office Kolkata",        officer_email = "rotest@gmail.com", applicant_name = "Suresh Mondal",    applicant_mobile = "9300000001", applicant_email = "suresh.m@example.com",  present_status = "PENDING",     applied_date = "2026-01-05", last_updated_date = "2026-01-05" },
            new() { acknowledgement_no = "BCW/2026/00002", application_no = "APP/2026/002", service_name = "BCW Welfare Scheme Service",      department_name = "BCW", office_name = "BCW District Office Kolkata",        officer_email = "rotest@gmail.com", applicant_name = "Lata Ghosh",       applicant_mobile = "9300000002", applicant_email = "lata.g@example.com",    present_status = "IN_PROGRESS", applied_date = "2026-01-08", last_updated_date = "2026-01-12" },
            new() { acknowledgement_no = "BCW/2026/00003", application_no = "APP/2026/003", service_name = "BCW Scholarship Grant Service",   department_name = "BCW", office_name = "BCW Block Office North 24 Parganas", officer_email = "dotest@gmail.com", applicant_name = "Pintu Das",        applicant_mobile = "9300000003", applicant_email = "pintu.d@example.com",   present_status = "DISPOSED",    applied_date = "2026-01-10", last_updated_date = "2026-02-01" },
            new() { acknowledgement_no = "BCW/2026/00004", application_no = "APP/2026/004", service_name = "BCW Caste Certificate Service",  department_name = "BCW", office_name = "BCW District Office Bardhaman",       officer_email = "aotest@gmail.com", applicant_name = "Mina Roy",         applicant_mobile = "9300000004", applicant_email = "mina.r@example.com",    present_status = "PENDING",     applied_date = "2026-01-15", last_updated_date = "2026-01-15" },
            new() { acknowledgement_no = "BCW/2026/00005", application_no = "APP/2026/005", service_name = "BCW Welfare Scheme Service",      department_name = "BCW", office_name = "BCW District Office Kolkata",        officer_email = "rotest@gmail.com", applicant_name = "Gopal Sen",        applicant_mobile = "9300000005", applicant_email = "gopal.s@example.com",   present_status = "IN_PROGRESS", applied_date = "2026-01-18", last_updated_date = "2026-01-22" },
            new() { acknowledgement_no = "BCW/2026/00006", application_no = "APP/2026/006", service_name = "BCW Scholarship Grant Service",   department_name = "BCW", office_name = "BCW Block Office North 24 Parganas", officer_email = "dotest@gmail.com", applicant_name = "Rina Pal",         applicant_mobile = "9300000006", applicant_email = "rina.p@example.com",    present_status = "DISPOSED",    applied_date = "2026-01-20", last_updated_date = "2026-02-10" },
            new() { acknowledgement_no = "BCW/2026/00007", application_no = "APP/2026/007", service_name = "BCW Caste Certificate Service",  department_name = "BCW", office_name = "BCW District Office Bardhaman",       officer_email = "aotest@gmail.com", applicant_name = "Bapi Biswas",      applicant_mobile = "9300000007", applicant_email = "bapi.b@example.com",    present_status = "PENDING",     applied_date = "2026-02-01", last_updated_date = "2026-02-01" },
            new() { acknowledgement_no = "BCW/2026/00008", application_no = "APP/2026/008", service_name = "BCW Welfare Scheme Service",      department_name = "BCW", office_name = "BCW District Office Kolkata",        officer_email = "rotest@gmail.com", applicant_name = "Soma Haldar",      applicant_mobile = "9300000008", applicant_email = "soma.h@example.com",    present_status = "IN_PROGRESS", applied_date = "2026-02-05", last_updated_date = "2026-02-08" },
            new() { acknowledgement_no = "BCW/2026/00009", application_no = "APP/2026/009", service_name = "BCW Scholarship Grant Service",   department_name = "BCW", office_name = "BCW Block Office North 24 Parganas", officer_email = "dotest@gmail.com", applicant_name = "Ratan Saha",       applicant_mobile = "9300000009", applicant_email = "ratan.s@example.com",   present_status = "DISPOSED",    applied_date = "2026-02-10", last_updated_date = "2026-03-01" },
            new() { acknowledgement_no = "BCW/2026/00010", application_no = "APP/2026/010", service_name = "BCW Caste Certificate Service",  department_name = "BCW", office_name = "BCW District Office Kolkata",        officer_email = "rotest@gmail.com", applicant_name = "Kabita Mondal",    applicant_mobile = "9300000010", applicant_email = "kabita.m@example.com",  present_status = "PENDING",     applied_date = "2026-02-15", last_updated_date = "2026-02-15" },
            new() { acknowledgement_no = "BCW/2026/00011", application_no = "APP/2026/011", service_name = "BCW Welfare Scheme Service",      department_name = "BCW", office_name = "BCW District Office Bardhaman",       officer_email = "aotest@gmail.com", applicant_name = "Dulal Karmakar",   applicant_mobile = "9300000011", applicant_email = "dulal.k@example.com",   present_status = "IN_PROGRESS", applied_date = "2026-02-20", last_updated_date = "2026-02-25" },
            new() { acknowledgement_no = "BCW/2026/00012", application_no = "APP/2026/012", service_name = "BCW Scholarship Grant Service",   department_name = "BCW", office_name = "BCW Block Office North 24 Parganas", officer_email = "dotest@gmail.com", applicant_name = "Parul Dey",        applicant_mobile = "9300000012", applicant_email = "parul.d@example.com",   present_status = "DISPOSED",    applied_date = "2026-03-01", last_updated_date = "2026-03-15" },
            new() { acknowledgement_no = "BCW/2026/00013", application_no = "APP/2026/013", service_name = "BCW Caste Certificate Service",  department_name = "BCW", office_name = "BCW District Office Kolkata",        officer_email = "rotest@gmail.com", applicant_name = "Nimai Chatterjee", applicant_mobile = "9300000013", applicant_email = "nimai.c@example.com",   present_status = "PENDING",     applied_date = "2026-03-05", last_updated_date = "2026-03-05" },
            new() { acknowledgement_no = "BCW/2026/00014", application_no = "APP/2026/014", service_name = "BCW Welfare Scheme Service",      department_name = "BCW", office_name = "BCW District Office Bardhaman",       officer_email = "aotest@gmail.com", applicant_name = "Jharna Bose",      applicant_mobile = "9300000014", applicant_email = "jharna.b@example.com",  present_status = "IN_PROGRESS", applied_date = "2026-03-10", last_updated_date = "2026-03-14" },
            new() { acknowledgement_no = "BCW/2026/00015", application_no = "APP/2026/015", service_name = "BCW Scholarship Grant Service",   department_name = "BCW", office_name = "BCW Block Office North 24 Parganas", officer_email = "dotest@gmail.com", applicant_name = "Tapan Mukherjee",  applicant_mobile = "9300000015", applicant_email = "tapan.m@example.com",  present_status = "DISPOSED",    applied_date = "2026-03-15", last_updated_date = "2026-04-01" },
        ];

        [HttpGet("offices")]
        public IActionResult GetOffices([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var auth = ValidateApiKey();
            if (auth != null) return auth;
            var (paged, total) = Paginate(_offices, page, page_size);
            _logger.LogInformation("[BCW-EXT] GET /offices → {Count}/{Total}", paged.Count, total);
            return Ok(BcwResponse<BcwOfficeDto>.From(paged, total));
        }

        [HttpGet("services")]
        public IActionResult GetServices([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var auth = ValidateApiKey();
            if (auth != null) return auth;
            var (paged, total) = Paginate(_services, page, page_size);
            _logger.LogInformation("[BCW-EXT] GET /services → {Count}/{Total}", paged.Count, total);
            return Ok(BcwResponse<BcwServiceDto>.From(paged, total));
        }

        [HttpGet("officers")]
        public IActionResult GetOfficers([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var auth = ValidateApiKey();
            if (auth != null) return auth;
            var (paged, total) = Paginate(_officers, page, page_size);
            _logger.LogInformation("[BCW-EXT] GET /officers → {Count}/{Total}", paged.Count, total);
            return Ok(BcwResponse<BcwOfficerDto>.From(paged, total));
        }

        [HttpGet("acknowledgements")]
        public IActionResult GetAcknowledgements([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var auth = ValidateApiKey();
            if (auth != null) return auth;
            var (paged, total) = Paginate(_acknowledgements, page, page_size);
            _logger.LogInformation("[BCW-EXT] GET /acknowledgements → {Count}/{Total}", paged.Count, total);
            return Ok(BcwResponse<BcwAcknowledgementDto>.From(paged, total));
        }

        private IActionResult? ValidateApiKey()
        {
            if (!Request.Headers.TryGetValue(_authHeaderName, out var value) || value != _authCredential)
            {
                _logger.LogWarning("[BCW-EXT] Unauthorized — missing or invalid {Header}", _authHeaderName);
                return Unauthorized(new { error = "Invalid or missing API key" });
            }
            return null;
        }

        private static (List<T> Page, int Total) Paginate<T>(List<T> all, int page, int pageSize)
        {
            var paged = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return (paged, all.Count);
        }
    }

    public class BcwResponse<T>
    {
        [JsonPropertyName("success")]    public bool    success    { get; set; }
        [JsonPropertyName("message")]    public string  message    { get; set; } = string.Empty;
        [JsonPropertyName("payload_id")] public string  payload_id { get; set; } = string.Empty;
        [JsonPropertyName("totalCount")] public int     totalCount { get; set; }
        [JsonPropertyName("data")]       public List<T> data       { get; set; } = [];

        public static BcwResponse<T> From(List<T> data, int total) => new()
        {
            success    = true,
            message    = "OK",
            payload_id = $"BCW-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8]}",
            totalCount = total,
            data       = data,
        };
    }

    public class BcwOfficeDto
    {
        [JsonPropertyName("office_name")]         public string  office_name         { get; set; } = default!;
        [JsonPropertyName("department_name")]      public string  department_name     { get; set; } = default!;
        [JsonPropertyName("office_level")]         public string  office_level        { get; set; } = default!;
        [JsonPropertyName("district_name")]        public string? district_name       { get; set; }
        [JsonPropertyName("block_name")]           public string? block_name          { get; set; }
        [JsonPropertyName("municipality_name")]    public string? municipality_name   { get; set; }
        [JsonPropertyName("gram_panchayat_name")]  public string? gram_panchayat_name { get; set; }
        [JsonPropertyName("ward_name")]            public string? ward_name           { get; set; }
        [JsonPropertyName("parent_office_name")]   public string? parent_office_name  { get; set; }
        [JsonPropertyName("is_active")]            public bool    is_active           { get; set; }
    }

    public class BcwServiceDto
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
        [JsonPropertyName("is_active")]        public bool    is_active        { get; set; }
    }

    public class BcwOfficerDto
    {
        [JsonPropertyName("officer_email")]   public string  officer_email   { get; set; } = default!;
        [JsonPropertyName("full_name")]       public string  full_name       { get; set; } = default!;
        [JsonPropertyName("mobile_no")]       public string? mobile_no       { get; set; }
        [JsonPropertyName("designation")]     public string  designation     { get; set; } = default!;
        [JsonPropertyName("role_key")]        public string  role_key        { get; set; } = default!;
        [JsonPropertyName("office_name")]     public string  office_name     { get; set; } = default!;
        [JsonPropertyName("department_name")] public string  department_name { get; set; } = default!;
        [JsonPropertyName("is_active")]       public bool    is_active       { get; set; }
    }

    public class BcwAcknowledgementDto
    {
        [JsonPropertyName("acknowledgement_no")] public string  acknowledgement_no { get; set; } = default!;
        [JsonPropertyName("application_no")]     public string? application_no     { get; set; }
        [JsonPropertyName("service_name")]       public string  service_name       { get; set; } = default!;
        [JsonPropertyName("department_name")]    public string  department_name    { get; set; } = default!;
        [JsonPropertyName("office_name")]        public string  office_name        { get; set; } = default!;
        [JsonPropertyName("officer_email")]      public string? officer_email      { get; set; }
        [JsonPropertyName("applicant_name")]     public string  applicant_name     { get; set; } = default!;
        [JsonPropertyName("applicant_mobile")]   public string? applicant_mobile   { get; set; }
        [JsonPropertyName("applicant_email")]    public string? applicant_email    { get; set; }
        [JsonPropertyName("present_status")]     public string  present_status     { get; set; } = default!;
        [JsonPropertyName("applied_date")]       public string? applied_date       { get; set; }
        [JsonPropertyName("last_updated_date")]  public string? last_updated_date  { get; set; }
    }
}
