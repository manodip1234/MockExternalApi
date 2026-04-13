// =============================================================================
// FILE:      MockApiController.cs
// PROJECT:   MockExternalApi
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace MockExternalApi.Controllers
{
    [ApiController]
    [Route("mock-api")]
    public class MockApiController : ControllerBase
    {
        // ─────────────────────────────────────────────────────────────
        // AUTH CONSTANTS
        // ─────────────────────────────────────────────────────────────
        private const string ApiKeyHeaderName    = "X-API-KEY";
        private const string TimestampHeaderName = "X-TIMESTAMP";
        private const string SignatureHeaderName = "X-HMAC-SIGNATURE";

        // API_KEY auth constant (shared with HMAC for key validation)
        private const string ValidApiKey         = "rtps-demo-key-001";

        // HMAC signing secret
        private const string HmacSecret          = "rtps-demo-secret-001";

        // ─────────────────────────────────────────────────────────────
        // GROUND TRUTH — values that MUST exist in the DB before sync
        // ─────────────────────────────────────────────────────────────

        private static readonly Dictionary<string, int> _deptMap = new()
        {
            ["BCW"]   = 4,
            ["FOOD"]  = 6,
            ["AGR"]   = 5,
            ["TRANS"] = 8,
            ["ENV"]   = 7,
            ["WRD"]   = 13,
        };

        private const int WestBengalStateId = 1;

        private static readonly Dictionary<string, int> _levelKeyToId = new()
        {
            ["STATE"]        = 1,
            ["DISTRICT"]     = 2,
            ["BLOCK"]        = 3,
            ["MUNICIPALITY"] = 4,
        };

        private static readonly Dictionary<string, int> _roleKeyToId = new()
        {
            ["DESIGNATED_OFFICER"] = 5,
            ["APPELLATE_OFFICER"]  = 6,
            ["REVIEWING_OFFICER"]  = 7,
        };

        private static readonly (int Id, string Name)[] _designations =
        [
            (1, "Additional Chief Secretary"),
            (2, "Principal Secretary"),
            (3, "Secretary"),
        ];

        private static readonly OfficeRef[] _realOffices =
        [
            // BCW
            new("OFF-BCW-1",   2, "BCW District Office Kolkata",           "Kolkata"),
            new("OFF-BCW-2",   1, "BCW State Office Kolkata",               "Kolkata"),
            new("OFF-BCW-3",   2, "BCW District Office Bardhaman",          "Bardhaman"),
            new("OFF-BCW-4",   3, "BCW Block Office North 24 Parganas",     "North 24 Parganas"),
            // FOOD
            new("OFF-FOOD-1",  2, "Food District Office Kolkata",           "Kolkata"),
            new("OFF-FOOD-2",  1, "Food State Office Kolkata",               "Kolkata"),
            new("OFF-FOOD-3",  2, "Food District Office Howrah",             "Howrah"),
            new("OFF-FOOD-4",  3, "Food Block Office Murshidabad",           "Murshidabad"),
            // AGR
            new("OFF-AGR-1",   2, "Agriculture District Office Kolkata",    "Kolkata"),
            new("OFF-AGR-2",   1, "Agriculture State Office Kolkata",        "Kolkata"),
            new("OFF-AGR-3",   2, "Agriculture District Office Nadia",       "Nadia"),
            new("OFF-AGR-4",   3, "Agriculture Block Office Birbhum",        "Birbhum"),
            // ENV
            new("OFF-ENV-1",   2, "Environment District Office Kolkata",    "Kolkata"),
            new("OFF-ENV-2",   1, "Environment State Office Kolkata",        "Kolkata"),
            new("OFF-ENV-3",   2, "Environment District Office Darjeeling",  "Darjeeling"),
            new("OFF-ENV-4",   3, "Environment Block Office Jalpaiguri",     "Jalpaiguri"),
            // TRANS
            new("OFF-TRANS-1", 2, "Transport District Office Kolkata",      "Kolkata"),
            new("OFF-TRANS-2", 1, "Transport State Office Kolkata",          "Kolkata"),
            new("OFF-TRANS-3", 2, "Transport District Office Asansol",       "Asansol"),
            new("OFF-TRANS-4", 3, "Transport Block Office Siliguri",         "Siliguri"),
            // WRD
            new("OFF-WRD-1",   2, "Water Resource District Office Kolkata", "Kolkata"),
            new("OFF-WRD-2",   1, "Water Resource State Office Kolkata",     "Kolkata"),
            new("OFF-WRD-3",   2, "Water Resource District Office Midnapore","Midnapore"),
            new("OFF-WRD-4",   3, "Water Resource Block Office Bankura",     "Bankura"),
        ];

        private static readonly ServiceRef[] _realServices =
        [
            new(2001, "BCW Service A",   30),
            new(2002, "BCW Service B",   45),
            new(2003, "BCW Service C",   20),
            new(3001, "Food Service A",  15),
            new(3002, "Food Service B",  25),
            new(3003, "Food Service C",  35),
            new(4001, "AGR Service A",   20),
            new(4002, "AGR Service B",   30),
            new(4003, "AGR Service C",   45),
            new(5001, "ENV Service A",   21),
            new(5002, "ENV Service B",   14),
            new(5003, "ENV Service C",   28),
            new(6001, "TRANS Service A", 10),
            new(6002, "TRANS Service B", 20),
            new(6003, "TRANS Service C", 30),
            new(7001, "WRD Service A",   25),
            new(7002, "WRD Service B",   40),
            new(7003, "WRD Service C",   15),
        ];

        // Per-department office/service slices
        private static OfficeRef[] OfficesFor(string dept) => dept.ToUpper() switch
        {
            "FOOD"  => _realOffices.Where(o => o.Code.StartsWith("OFF-FOOD")).ToArray(),
            "AGR"   => _realOffices.Where(o => o.Code.StartsWith("OFF-AGR")).ToArray(),
            "ENV"   => _realOffices.Where(o => o.Code.StartsWith("OFF-ENV")).ToArray(),
            "TRANS" => _realOffices.Where(o => o.Code.StartsWith("OFF-TRANS")).ToArray(),
            "WRD"   => _realOffices.Where(o => o.Code.StartsWith("OFF-WRD")).ToArray(),
            _       => _realOffices.Where(o => o.Code.StartsWith("OFF-BCW")).ToArray(),
        };

        private static ServiceRef[] ServicesFor(string dept) => dept.ToUpper() switch
        {
            "FOOD"  => _realServices.Where(s => s.Code >= 3001 && s.Code <= 3999).ToArray(),
            "AGR"   => _realServices.Where(s => s.Code >= 4001 && s.Code <= 4999).ToArray(),
            "ENV"   => _realServices.Where(s => s.Code >= 5001 && s.Code <= 5999).ToArray(),
            "TRANS" => _realServices.Where(s => s.Code >= 6001 && s.Code <= 6999).ToArray(),
            "WRD"   => _realServices.Where(s => s.Code >= 7001 && s.Code <= 7999).ToArray(),
            _       => _realServices.Where(s => s.Code >= 2001 && s.Code <= 2999).ToArray(),
        };

        private readonly ILogger<MockApiController> _logger;

        public MockApiController(ILogger<MockApiController> logger)
        {
            _logger = logger;
        }

        private static string OfficerEmail(int n, string deptCode)
            => $"officer{n}.{deptCode.ToLower()}@wb.gov.in";

        private int ResolveDeptId(string deptCode)
            => _deptMap.TryGetValue(deptCode.ToUpper(), out var id) ? id : 4;

        // Stable payload_id — changes only once per day per pipeline
        private static string StablePayloadId(string dept, string type)
            => $"P-{dept.ToUpper()}-{type}-{DateTime.UtcNow:yyyyMMddHH}";

        // Returns the paged slice; totalCount is always the full list size
        private static (List<T> Page, int TotalCount) Paginate<T>(List<T> all, int page, int pageSize)
        {
            var total  = all.Count;
            var paged  = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return (paged, total);
        }

        // ─────────────────────────────────────────────────────────────
        // OFFICE endpoint  →  auth: HMAC
        // ─────────────────────────────────────────────────────────────
        [HttpGet("office/{departmentCode}")]
        public IActionResult GetOffice(string departmentCode,
            [FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            return BuildOfficeResponse(departmentCode, page, page_size);
        }

        private IActionResult BuildOfficeResponse(string departmentCode, int page, int pageSize)
        {
            var deptId = ResolveDeptId(departmentCode);
            var all = OfficesFor(departmentCode).Select(o => new OfficeDto
            {
                office_code      = o.Code,
                office_name      = o.Name,
                department_code  = departmentCode.ToUpper(),
                department_id    = deptId,
                district_name    = o.DistrictName,
                state_name       = "West Bengal",
                state_id         = WestBengalStateId,
                level_id         = o.LevelId,
                level_key        = _levelKeyToId.First(kv => kv.Value == o.LevelId).Key,
                parent_office_id = null,
                is_active        = true,
            }).ToList();
            var (paged, total) = Paginate(all, page, pageSize);
            return Ok(ApiResponse<OfficeDto>.Ok(paged, total, "Office data", StablePayloadId(departmentCode, "OFFICE")));
        }

        // ─────────────────────────────────────────────────────────────
        // SERVICE endpoint  →  auth: HMAC
        // ─────────────────────────────────────────────────────────────
        [HttpGet("service/{departmentCode}")]
        public IActionResult GetService(string departmentCode,
            [FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            return BuildServiceResponse(departmentCode, page, page_size);
        }

        private IActionResult BuildServiceResponse(string departmentCode, int page, int pageSize)
        {
            var all = ServicesFor(departmentCode).Select(s => new ServiceDto
            {
                service_code     = s.Code,
                service_name     = s.Name,
                department_code  = departmentCode.ToUpper(),
                department_id    = ResolveDeptId(departmentCode),
                stipulated_days  = s.StimulateDays,
                stipulated_text  = "Standard processing",
                stipulated_hours = null,
                resolution_days  = s.StimulateDays + 5,
                resolution_hours = null,
                appeal_days      = 30,
                appeal_hours     = null,
                reappeal_days    = 60,
                reappeal_hours   = null,
                is_active        = true,
            }).ToList();
            var (paged, total) = Paginate(all, page, pageSize);
            return Ok(ApiResponse<ServiceDto>.Ok(paged, total, "Service data", StablePayloadId(departmentCode, "SERVICE")));
        }

        // ─────────────────────────────────────────────────────────────
        // USER endpoint  →  auth: HMAC
        // ─────────────────────────────────────────────────────────────
        [HttpGet("user/{departmentCode}")]
        public IActionResult GetUser(string departmentCode,
            [FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            return BuildUserResponse(departmentCode, page, page_size);
        }

        private IActionResult BuildUserResponse(string departmentCode, int page, int pageSize)
        {
            if (departmentCode.Equals("EMPTY", StringComparison.OrdinalIgnoreCase))
                return Ok(ApiResponse<UserDto>.Empty("No users found"));

            var deptOffices = OfficesFor(departmentCode);
            var all = new List<UserDto>
            {
                new() { official_email = OfficerEmail(1, departmentCode), full_name = "Ayan Chakraborty", mobile_no = "9800000001", designation = "Additional Chief Secretary", role_key = "DESIGNATED_OFFICER", office_code = deptOffices[0].Code,                      department_code = departmentCode.ToUpper(), is_active = true },
                new() { official_email = OfficerEmail(2, departmentCode), full_name = "Priya Banerjee",   mobile_no = "9800000002", designation = "Principal Secretary",        role_key = "APPELLATE_OFFICER",  office_code = deptOffices[1 % deptOffices.Length].Code, department_code = departmentCode.ToUpper(), is_active = true },
                new() { official_email = OfficerEmail(3, departmentCode), full_name = "Suresh Mondal",    mobile_no = "9800000003", designation = "Secretary",                  role_key = "REVIEWING_OFFICER",  office_code = deptOffices[2 % deptOffices.Length].Code, department_code = departmentCode.ToUpper(), is_active = true },
            };
            var (paged, total) = Paginate(all, page, pageSize);
            return Ok(ApiResponse<UserDto>.Ok(paged, total, "User data", StablePayloadId(departmentCode, "USER")));
        }

        // ─────────────────────────────────────────────────────────────
        // ACKNOWLEDGEMENT endpoint  →  auth: HMAC
        // ─────────────────────────────────────────────────────────────
        [HttpGet("acknowledgement/{departmentCode}")]
        [HttpGet("ack/{departmentCode}")]
        public IActionResult GetAcknowledgement(string departmentCode,
            [FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            return BuildAckResponse(departmentCode, page, page_size);
        }

        private IActionResult BuildAckResponse(string departmentCode, int page, int pageSize)
        {
            if (departmentCode.Equals("EMPTY", StringComparison.OrdinalIgnoreCase))
                return Ok(ApiResponse<AcknowledgementDto>.Empty("No acknowledgements found"));

            var statuses     = new[] { "IN_PROGRESS", "DISPOSED", "REJECTED", "PENDING" };
            var deptOffices  = OfficesFor(departmentCode);
            var deptServices = ServicesFor(departmentCode);
            var all = Enumerable.Range(1, 10).Select(i =>
            {
                var svc = deptServices[(i - 1) % deptServices.Length];
                var off = deptOffices[(i - 1)  % deptOffices.Length];
                return new AcknowledgementDto
                {
                    acknowledgement_no = (545433 + i).ToString(),
                    application_no     = (645433 + i).ToString(),
                    applicant_name     = SampleApplicantName(i),
                    applicant_mobile   = $"980000000{i}",
                    applicant_email    = $"citizen{i}@example.com",
                    service_code       = svc.Code,
                    office_code        = off.Code,
                    official_email     = OfficerEmail((i % 3) + 1, departmentCode),
                    department_code    = departmentCode.ToUpper(),
                    present_status     = statuses[(i - 1) % statuses.Length],
                    applied_date       = DateTime.UtcNow.AddDays(-(i * 5)).ToString("yyyy-MM-dd"),
                    last_updated_date  = DateTime.UtcNow.AddDays(-(i * 2)).ToString("yyyy-MM-dd"),
                };
            }).ToList();
            var (paged, total) = Paginate(all, page, pageSize);
            return Ok(ApiResponse<AcknowledgementDto>.Ok(paged, total, "Acknowledgement data", StablePayloadId(departmentCode, "ACK")));
        }

        // ═══════════════════════════════════════════════════════════════
        // DEPARTMENT-SPECIFIC ENDPOINTS
        // ═══════════════════════════════════════════════════════════════

        // ─────────────────────────────────────────────────────────────
        // BCW DEPARTMENT ENDPOINTS
        // ─────────────────────────────────────────────────────────────
        [HttpGet("bcw/office")]
        public IActionResult GetBcwOffice([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[BCW-OFFICE] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildOfficeResponse("BCW", page, page_size);
        }

        [HttpGet("bcw/service")]
        public IActionResult GetBcwService([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[BCW-SERVICE] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildServiceResponse("BCW", page, page_size);
        }

        [HttpGet("bcw/user")]
        public IActionResult GetBcwUser([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[BCW-USER] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildUserResponse("BCW", page, page_size);
        }

        [HttpGet("bcw/acknowledgement")]
        [HttpGet("bcw/ack")]
        public IActionResult GetBcwAcknowledgement([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[BCW-ACK] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildAckResponse("BCW", page, page_size);
        }

        // ─────────────────────────────────────────────────────────────
        // FOOD DEPARTMENT ENDPOINTS
        // ─────────────────────────────────────────────────────────────
        [HttpGet("food/office")]
        public IActionResult GetFoodOffice([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[FOOD-OFFICE] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildOfficeResponse("FOOD", page, page_size);
        }

        [HttpGet("food/service")]
        public IActionResult GetFoodService([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[FOOD-SERVICE] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildServiceResponse("FOOD", page, page_size);
        }

        [HttpGet("food/user")]
        public IActionResult GetFoodUser([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[FOOD-USER] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildUserResponse("FOOD", page, page_size);
        }

        [HttpGet("food/acknowledgement")]
        [HttpGet("food/ack")]
        public IActionResult GetFoodAcknowledgement([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[FOOD-ACK] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildAckResponse("FOOD", page, page_size);
        }

        // ─────────────────────────────────────────────────────────────
        // AGR DEPARTMENT ENDPOINTS
        // ─────────────────────────────────────────────────────────────
        [HttpGet("agr/office")]
        public IActionResult GetAgrOffice([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[AGR-OFFICE] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildOfficeResponse("AGR", page, page_size);
        }

        [HttpGet("agr/service")]
        public IActionResult GetAgrService([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[AGR-SERVICE] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildServiceResponse("AGR", page, page_size);
        }

        [HttpGet("agr/user")]
        public IActionResult GetAgrUser([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[AGR-USER] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildUserResponse("AGR", page, page_size);
        }

        [HttpGet("agr/acknowledgement")]
        [HttpGet("agr/ack")]
        public IActionResult GetAgrAcknowledgement([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[AGR-ACK] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildAckResponse("AGR", page, page_size);
        }

        // ─────────────────────────────────────────────────────────────
        // ENV  —  SCENARIO: Happy path (all valid, all FKs resolvable)
        // ─────────────────────────────────────────────────────────────
        [HttpGet("env/office")]
        public IActionResult GetEnvOffice([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[ENV-OFFICE] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<OfficeDto>
            {
                new() { office_code = "OFF-ENV-1", office_name = "Environment District Office Kolkata",    department_code = "ENV", department_id = 7, district_name = "Kolkata",    state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_code = "OFF-ENV-2", office_name = "Environment State Office Kolkata",      department_code = "ENV", department_id = 7, district_name = "Kolkata",    state_name = "West Bengal", state_id = WestBengalStateId, level_id = 1, level_key = "STATE",    is_active = true },
                new() { office_code = "OFF-ENV-3", office_name = "Environment District Office Darjeeling",department_code = "ENV", department_id = 7, district_name = "Darjeeling", state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_code = "OFF-ENV-4", office_name = "Environment Block Office Jalpaiguri",   department_code = "ENV", department_id = 7, district_name = "Jalpaiguri", state_name = "West Bengal", state_id = WestBengalStateId, level_id = 3, level_key = "BLOCK",    is_active = true },
            };
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<OfficeDto>.Ok(paged, total, "Office data", StablePayloadId("ENV", "OFFICE")));
        }

        [HttpGet("env/service")]
        public IActionResult GetEnvService([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[ENV-SERVICE] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<ServiceDto>
            {
                new() { service_code = 5001, service_name = "ENV Service A", department_code = "ENV", department_id = 7, stipulated_days = 21, resolution_days = 26, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_code = 5002, service_name = "ENV Service B", department_code = "ENV", department_id = 7, stipulated_days = 14, resolution_days = 19, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_code = 5003, service_name = "ENV Service C", department_code = "ENV", department_id = 7, stipulated_days = 28, resolution_days = 33, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
            };
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<ServiceDto>.Ok(paged, total, "Service data", StablePayloadId("ENV", "SERVICE")));
        }

        [HttpGet("env/user")]
        public IActionResult GetEnvUser([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[ENV-USER] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<UserDto>
            {
                new() { official_email = "officer1.env@wb.gov.in", full_name = "Ananya Das",       mobile_no = "9811000001", designation = "Additional Chief Secretary", role_key = "DESIGNATED_OFFICER", office_code = "OFF-ENV-1", department_code = "ENV", is_active = true },
                new() { official_email = "officer2.env@wb.gov.in", full_name = "Debashis Roy",     mobile_no = "9811000002", designation = "Principal Secretary",        role_key = "APPELLATE_OFFICER",  office_code = "OFF-ENV-2", department_code = "ENV", is_active = true },
                new() { official_email = "officer3.env@wb.gov.in", full_name = "Mitali Ghosh",     mobile_no = "9811000003", designation = "Secretary",                  role_key = "REVIEWING_OFFICER",  office_code = "OFF-ENV-3", department_code = "ENV", is_active = true },
            };
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<UserDto>.Ok(paged, total, "User data", StablePayloadId("ENV", "USER")));
        }

        [HttpGet("env/acknowledgement")]
        [HttpGet("env/ack")]
        public IActionResult GetEnvAcknowledgement([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[ENV-ACK] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var statuses = new[] { "DISPOSED", "DISPOSED", "DISPOSED", "IN_PROGRESS" };
            var all = Enumerable.Range(1, 6).Select(i => new AcknowledgementDto
            {
                acknowledgement_no = $"ENV-ACK-{i:D4}",
                application_no     = $"ENV-APP-{i:D4}",
                applicant_name     = SampleApplicantName(i),
                applicant_mobile   = $"9811100{i:D3}",
                applicant_email    = $"citizen.env{i}@example.com",
                service_code       = 5001 + ((i - 1) % 3),
                office_code        = $"OFF-ENV-{((i - 1) % 4) + 1}",
                official_email     = $"officer{((i % 3) + 1)}.env@wb.gov.in",
                department_code    = "ENV",
                present_status     = statuses[(i - 1) % statuses.Length],
                applied_date       = DateTime.UtcNow.AddDays(-(i * 3)).ToString("yyyy-MM-dd"),
                last_updated_date  = DateTime.UtcNow.AddDays(-i).ToString("yyyy-MM-dd"),
            }).ToList();
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<AcknowledgementDto>.Ok(paged, total, "Acknowledgement data", StablePayloadId("ENV", "ACK")));
        }

        // ─────────────────────────────────────────────────────────────
        // TRANS  —  SCENARIO: Partial FK failure
        //   Offices 3 & 4 use codes that do NOT exist in rtps_wb.office
        //   ACK records 5-8 reference those ghost offices → retry queue
        // ─────────────────────────────────────────────────────────────
        [HttpGet("trans/office")]
        public IActionResult GetTransOffice([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[TRANS-OFFICE] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<OfficeDto>
            {
                new() { office_code = "OFF-TRANS-1",       office_name = "Transport District Office Kolkata",  department_code = "TRANS", department_id = 8, district_name = "Kolkata",  state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_code = "OFF-TRANS-2",       office_name = "Transport State Office Kolkata",    department_code = "TRANS", department_id = 8, district_name = "Kolkata",  state_name = "West Bengal", state_id = WestBengalStateId, level_id = 1, level_key = "STATE",    is_active = true },
                // These two codes do NOT exist in rtps_wb.office
                new() { office_code = "OFF-TRANS-GHOST-1", office_name = "Transport Ghost Office Asansol",    department_code = "TRANS", department_id = 8, district_name = "Asansol",  state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_code = "OFF-TRANS-GHOST-2", office_name = "Transport Ghost Office Siliguri",   department_code = "TRANS", department_id = 8, district_name = "Siliguri", state_name = "West Bengal", state_id = WestBengalStateId, level_id = 3, level_key = "BLOCK",    is_active = true },
            };
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<OfficeDto>.Ok(paged, total, "Office data", StablePayloadId("TRANS", "OFFICE")));
        }

        [HttpGet("trans/service")]
        public IActionResult GetTransService([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[TRANS-SERVICE] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<ServiceDto>
            {
                new() { service_code = 6001, service_name = "TRANS Service A", department_code = "TRANS", department_id = 8, stipulated_days = 10, resolution_days = 15, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_code = 6002, service_name = "TRANS Service B", department_code = "TRANS", department_id = 8, stipulated_days = 20, resolution_days = 25, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_code = 6003, service_name = "TRANS Service C", department_code = "TRANS", department_id = 8, stipulated_days = 30, resolution_days = 35, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
            };
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<ServiceDto>.Ok(paged, total, "Service data", StablePayloadId("TRANS", "SERVICE")));
        }

        [HttpGet("trans/user")]
        public IActionResult GetTransUser([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[TRANS-USER] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<UserDto>
            {
                new() { official_email = "officer1.trans@wb.gov.in", full_name = "Rajesh Sinha",    mobile_no = "9822000001", designation = "Additional Chief Secretary", role_key = "DESIGNATED_OFFICER", office_code = "OFF-TRANS-1",       department_code = "TRANS", is_active = true },
                new() { official_email = "officer2.trans@wb.gov.in", full_name = "Soma Chatterjee", mobile_no = "9822000002", designation = "Principal Secretary",        role_key = "APPELLATE_OFFICER",  office_code = "OFF-TRANS-2",       department_code = "TRANS", is_active = true },
                // References ghost office — FK will fail
                new() { official_email = "officer3.trans@wb.gov.in", full_name = "Nikhil Bose",     mobile_no = "9822000003", designation = "Secretary",                  role_key = "REVIEWING_OFFICER",  office_code = "OFF-TRANS-GHOST-1", department_code = "TRANS", is_active = true },
            };
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<UserDto>.Ok(paged, total, "User data", StablePayloadId("TRANS", "USER")));
        }

        [HttpGet("trans/acknowledgement")]
        [HttpGet("trans/ack")]
        public IActionResult GetTransAcknowledgement([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[TRANS-ACK] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = Enumerable.Range(1, 8).Select(i => new AcknowledgementDto
            {
                acknowledgement_no = $"TRANS-ACK-{i:D4}",
                application_no     = $"TRANS-APP-{i:D4}",
                applicant_name     = SampleApplicantName(i),
                applicant_mobile   = $"9822200{i:D3}",
                applicant_email    = $"citizen.trans{i}@example.com",
                service_code       = 6001 + ((i - 1) % 3),
                // Records 5-8 reference ghost office codes → FK unresolvable → retry queue
                office_code        = i <= 4 ? $"OFF-TRANS-{i}" : $"OFF-TRANS-GHOST-{i - 4}",
                official_email     = i <= 4 ? $"officer{((i % 2) + 1)}.trans@wb.gov.in" : "officer3.trans@wb.gov.in",
                department_code    = "TRANS",
                present_status     = i % 2 == 0 ? "DISPOSED" : "IN_PROGRESS",
                applied_date       = DateTime.UtcNow.AddDays(-(i * 4)).ToString("yyyy-MM-dd"),
                last_updated_date  = DateTime.UtcNow.AddDays(-(i * 2)).ToString("yyyy-MM-dd"),
            }).ToList();
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<AcknowledgementDto>.Ok(paged, total, "Acknowledgement data", StablePayloadId("TRANS", "ACK")));
        }

        // ─────────────────────────────────────────────────────────────
        // WRD  —  SCENARIO: Edge cases
        //   USER → empty list (tests empty-dataset handling)
        //   ACK  → 25 records (tests pagination); records 21-25 reuse
        //          ack numbers from 1-5 (tests duplicate handling)
        //   ACK  → null official_email (tests null FK handling)
        // ─────────────────────────────────────────────────────────────
        [HttpGet("wrd/office")]
        public IActionResult GetWrdOffice([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[WRD-OFFICE] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<OfficeDto>
            {
                new() { office_code = "OFF-WRD-1", office_name = "Water Resource District Office Kolkata",   department_code = "WRD", department_id = 13, district_name = "Kolkata",   state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_code = "OFF-WRD-2", office_name = "Water Resource State Office Kolkata",     department_code = "WRD", department_id = 13, district_name = "Kolkata",   state_name = "West Bengal", state_id = WestBengalStateId, level_id = 1, level_key = "STATE",    is_active = true },
                new() { office_code = "OFF-WRD-3", office_name = "Water Resource District Office Midnapore",department_code = "WRD", department_id = 13, district_name = "Midnapore", state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_code = "OFF-WRD-4", office_name = "Water Resource Block Office Bankura",     department_code = "WRD", department_id = 13, district_name = "Bankura",   state_name = "West Bengal", state_id = WestBengalStateId, level_id = 3, level_key = "BLOCK",    is_active = true },
            };
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<OfficeDto>.Ok(paged, total, "Office data", StablePayloadId("WRD", "OFFICE")));
        }

        [HttpGet("wrd/service")]
        public IActionResult GetWrdService([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[WRD-SERVICE] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<ServiceDto>
            {
                new() { service_code = 7001, service_name = "WRD Service A", department_code = "WRD", department_id = 13, stipulated_days = 25, resolution_days = 30, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_code = 7002, service_name = "WRD Service B", department_code = "WRD", department_id = 13, stipulated_days = 40, resolution_days = 45, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_code = 7003, service_name = "WRD Service C", department_code = "WRD", department_id = 13, stipulated_days = 15, resolution_days = 20, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
            };
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<ServiceDto>.Ok(paged, total, "Service data", StablePayloadId("WRD", "SERVICE")));
        }

        // WRD USER — empty list to test empty-dataset handling
        [HttpGet("wrd/user")]
        public IActionResult GetWrdUser([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[WRD-USER] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path} | Scenario=EMPTY", page, page_size, path);

            return Ok(ApiResponse<UserDto>.Empty("No users found for WRD"));
        }

        // WRD ACK — 25 records; records 21-25 reuse ack numbers 1-5 (duplicate test)
        [HttpGet("wrd/acknowledgement")]
        [HttpGet("wrd/ack")]
        public IActionResult GetWrdAcknowledgement([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[WRD-ACK] ✓ Request passed | Page={Page} PageSize={PageSize} | Path={Path} | Scenario=PAGINATION+DUPLICATE", page, page_size, path);

            var statuses = new[] { "IN_PROGRESS", "DISPOSED", "REJECTED", "PENDING" };
            var all = Enumerable.Range(1, 25).Select(i =>
            {
                var ackNo = i <= 20 ? $"WRD-ACK-{i:D4}" : $"WRD-ACK-{(i - 20):D4}";
                return new AcknowledgementDto
                {
                    acknowledgement_no = ackNo,
                    application_no     = $"WRD-APP-{i:D4}",
                    applicant_name     = SampleApplicantName((i - 1) % 10 + 1),
                    applicant_mobile   = $"9833300{i:D3}",
                    applicant_email    = $"citizen.wrd{i}@example.com",
                    service_code       = 7001 + ((i - 1) % 3),
                    office_code        = $"OFF-WRD-{((i - 1) % 4) + 1}",
                    official_email     = null, // null officer email — tests null FK handling
                    department_code    = "WRD",
                    present_status     = statuses[(i - 1) % statuses.Length],
                    applied_date       = DateTime.UtcNow.AddDays(-(i * 2)).ToString("yyyy-MM-dd"),
                    last_updated_date  = DateTime.UtcNow.AddDays(-i).ToString("yyyy-MM-dd"),
                };
            }).ToList();
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<AcknowledgementDto>.Ok(paged, total, "Acknowledgement data", StablePayloadId("WRD", "ACK")));
        }

        // ─────────────────────────────────────────────────────────────
        // DEPARTMENT-SPECIFIC CALLBACK ENDPOINTS
        // ─────────────────────────────────────────────────────────────
        [HttpPost("bcw/sync-response")]
        public Task<IActionResult> BcwCallback()   => ReceiveSyncCallback();

        [HttpPost("food/sync-response")]
        public Task<IActionResult> FoodCallback()  => ReceiveSyncCallback();

        [HttpPost("agr/sync-response")]
        public Task<IActionResult> AgrCallback()   => ReceiveSyncCallback();

        [HttpPost("env/sync-response")]
        public Task<IActionResult> EnvCallback()   => ReceiveSyncCallback();

        [HttpPost("trans/sync-response")]
        public Task<IActionResult> TransCallback() => ReceiveSyncCallback();

        [HttpPost("wrd/sync-response")]
        public Task<IActionResult> WrdCallback()   => ReceiveSyncCallback();

        // ─────────────────────────────────────────────────────────────
        // CALLBACK receiver  →  POST /mock-api/sync-response (shared)
        // ─────────────────────────────────────────────────────────────
        [HttpPost("sync-response")]
        public async Task<IActionResult> ReceiveSyncCallback()
        {
            // Enable buffering so body can be read multiple times
            Request.EnableBuffering();

            // Read raw body
            using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
            var rawBody = await reader.ReadToEndAsync();
            Request.Body.Position = 0;

            var path      = Request.Path.ToString();
            var timestamp = Request.Headers.TryGetValue("X-TIMESTAMP", out var ts) ? ts.ToString() : string.Empty;
            var apiKey    = Request.Headers.TryGetValue("X-API-KEY",   out var ak) ? ak.ToString() : string.Empty;
            var incomingSig = Request.Headers.TryGetValue("X-HMAC-SIGNATURE", out var sig) ? sig.ToString() : string.Empty;

            _logger.LogInformation("╔══════════════════════════════════════════════════════════╗");
            _logger.LogInformation("║           [CALLBACK-RECEIVED] INCOMING POST              ║");
            _logger.LogInformation("╚══════════════════════════════════════════════════════════╝");
            _logger.LogInformation("[CALLBACK-RECEIVED] Path      : {Path}", path);
            _logger.LogInformation("[CALLBACK-RECEIVED] X-API-KEY : {Key}",  apiKey);
            _logger.LogInformation("[CALLBACK-RECEIVED] X-TIMESTAMP: {Ts}",  timestamp);
            _logger.LogInformation("[CALLBACK-RECEIVED] X-HMAC-SIGNATURE: {Sig}", incomingSig);
            _logger.LogInformation("[CALLBACK-RECEIVED] Raw Body  : {Body}", rawBody);

            // ── HMAC Validation ──────────────────────────────────────────
            if (string.IsNullOrWhiteSpace(apiKey) || apiKey != ValidApiKey)
            {
                _logger.LogWarning("[CALLBACK-REJECTED] Invalid or missing X-API-KEY");
                return Unauthorized("CALLBACK: Invalid API Key");
            }

            if (string.IsNullOrWhiteSpace(timestamp))
            {
                _logger.LogWarning("[CALLBACK-REJECTED] Missing X-TIMESTAMP");
                return Unauthorized("CALLBACK: X-TIMESTAMP missing");
            }

            if (string.IsNullOrWhiteSpace(incomingSig))
            {
                _logger.LogWarning("[CALLBACK-REJECTED] Missing X-HMAC-SIGNATURE");
                return Unauthorized("CALLBACK: X-HMAC-SIGNATURE missing");
            }

            // Rebuild signature string: POST|{path}|{timestamp}|{rawBody}
            var rawSignatureString = $"POST|{path}|{timestamp}|{rawBody}";
            _logger.LogInformation("[CALLBACK-RECEIVED] Signature String: {Str}", rawSignatureString);

            var keyBytes     = Encoding.UTF8.GetBytes(HmacSecret);
            var msgBytes     = Encoding.UTF8.GetBytes(rawSignatureString);
            using var hmac   = new HMACSHA256(keyBytes);
            var hashBytes    = hmac.ComputeHash(msgBytes);
            var expectedSig  = Convert.ToBase64String(hashBytes);

            _logger.LogInformation("[CALLBACK-RECEIVED] Expected Signature : {Exp}", expectedSig);
            _logger.LogInformation("[CALLBACK-RECEIVED] Incoming Signature : {Inc}", incomingSig);

            if (!CryptographicOperations.FixedTimeEquals(
                    Encoding.UTF8.GetBytes(expectedSig),
                    Encoding.UTF8.GetBytes(incomingSig)))
            {
                _logger.LogWarning("[CALLBACK-REJECTED] ✗ HMAC signature mismatch | Path={Path} | Timestamp={Ts}", path, timestamp);
                return Unauthorized("CALLBACK: Signature mismatch");
            }

            _logger.LogInformation("[CALLBACK-RECEIVED] ✓ HMAC signature VALID");

            // ── Deserialize payload ───────────────────────────────────────
            CallbackPayload? payload = null;
            try
            {
                payload = System.Text.Json.JsonSerializer.Deserialize<CallbackPayload>(
                    rawBody,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[CALLBACK-RECEIVED] Failed to deserialize body");
                return BadRequest("Invalid JSON body");
            }

            if (payload == null)
            {
                _logger.LogWarning("[CALLBACK-RECEIVED] Payload is null after deserialization");
                return BadRequest("Empty payload");
            }

            // ── Extract pipeline name from payload_id ─────────────────────
            // Format: P-BCW-OFFICE-20260408095646  →  pipeline = OFFICE
            var pipeline = "UNKNOWN";
            if (!string.IsNullOrWhiteSpace(payload.PayloadId))
            {
                var parts = payload.PayloadId.Split('-');
                if (parts.Length >= 3) pipeline = parts[2];
            }

            // ── Structured log summary ────────────────────────────────────
            _logger.LogInformation("┌──────────────────────────────────────────────────────────┐");
            _logger.LogInformation("│              [CALLBACK-RECEIVED] SUMMARY                 │");
            _logger.LogInformation("├──────────────────────────────────────────────────────────┤");
            _logger.LogInformation("│ Pipeline   : {Pipeline,-52} │", pipeline);
            _logger.LogInformation("│ PayloadId  : {Id,-52} │", payload.PayloadId ?? "[NULL]");
            _logger.LogInformation("│ Status     : {Status,-52} │", payload.Status);
            _logger.LogInformation("│ Total      : {Total,-52} │", payload.TotalRecords);
            _logger.LogInformation("│ Success    : {Success,-52} │", payload.SuccessCount);
            _logger.LogInformation("│ Failed     : {Failed,-52} │", payload.FailureCount);
            _logger.LogInformation("└──────────────────────────────────────────────────────────┘");

            if (payload.Errors?.Count > 0)
            {
                _logger.LogWarning("[CALLBACK-RECEIVED] Errors ({Count}):", payload.Errors.Count);
                for (int i = 0; i < payload.Errors.Count; i++)
                    _logger.LogWarning("[CALLBACK-RECEIVED]   [{N}] ReferenceId={Ref} | Message={Msg}",
                        i + 1, payload.Errors[i].ReferenceId, payload.Errors[i].ErrorMessage);
            }
            else
            {
                _logger.LogInformation("[CALLBACK-RECEIVED] Errors: NONE");
            }

            // ── Pretty print to console ───────────────────────────────────
            try
            {
                var parsed = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(rawBody);
                var pretty = System.Text.Json.JsonSerializer.Serialize(parsed,
                    new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine("\n[CALLBACK-RECEIVED] Pretty Payload:");
                Console.WriteLine(pretty);
                Console.WriteLine();
            }
            catch
            {
                Console.WriteLine("[CALLBACK-RECEIVED] Raw: " + rawBody);
            }

            return Ok(new
            {
                received   = true,
                payload_id = payload.PayloadId,
                pipeline,
                status     = payload.Status,
                timestamp  = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            });
        }
        // ─────────────────────────────────────────────────────────────
        // AUTH: UNIFIED DISPATCHER
        //
        // authType | behaviour
        // ---------|--------------------------------------------------
        // "NONE"   | Always passes — returns null
        // "API_KEY"| Delegates to ValidateApiKey()
        // "HMAC"   | Delegates to ValidateHmac(path)
        // other    | Returns 401 Unsupported auth type
        // ─────────────────────────────────────────────────────────────
        private IActionResult? ValidateRequest(string authType, string path) =>
            authType.ToUpperInvariant() switch
            {
                "NONE"    => null,
                "API_KEY" => ValidateApiKey(),
                "HMAC"    => ValidateHmac(path),
                _         => Unauthorized($"Unsupported auth type: {authType}"),
            };

        // ─────────────────────────────────────────────────────────────
        // AUTH: API KEY
        //
        // Reads header X-API-KEY and compares against ValidApiKey.
        // Returns null on success, Unauthorized on failure.
        // ─────────────────────────────────────────────────────────────
        private IActionResult? ValidateApiKey()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var key))
                return Unauthorized("API Key missing");

            if (key != ValidApiKey)
                return Unauthorized("Invalid API Key");

            return null; // valid
        }

        // ─────────────────────────────────────────────────────────────
        // AUTH: HMAC SHA-256
        //
        // Expected headers:
        //   X-API-KEY        — must equal ValidApiKey
        //   X-TIMESTAMP      — ISO / epoch string included in raw string
        //   X-HMAC-SIGNATURE — Base64(HMAC-SHA256(raw, HmacSecret))
        //
        // Raw string format (pipe-separated, no spaces around pipe):
        //   METHOD|PATH_AND_QUERY|TIMESTAMP|BODY
        //
        // For GET requests BODY is always "{}".
        // Returns null on success, Unauthorized on failure.
        // ─────────────────────────────────────────────────────────────
        private IActionResult? ValidateHmac(string path, string body = "")
        {
            // 1. Validate API key presence and value
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey))
                return Unauthorized("HMAC: X-API-KEY header missing");

            if (apiKey != ValidApiKey)
                return Unauthorized("HMAC: Invalid API Key");

            // 2. Read and validate timestamp
            if (!Request.Headers.TryGetValue(TimestampHeaderName, out var timestamp)
                || string.IsNullOrWhiteSpace(timestamp))
                return Unauthorized("HMAC: X-TIMESTAMP header missing");

            if (DateTime.TryParse(timestamp, null, System.Globalization.DateTimeStyles.RoundtripKind, out var parsedTs))
            {
                var drift = Math.Abs((DateTime.UtcNow - parsedTs.ToUniversalTime()).TotalMinutes);
                if (drift > 5)
                    return StatusCode(403, new { error = "Timestamp expired" });
            }

            // 3. Read incoming signature
            if (!Request.Headers.TryGetValue(SignatureHeaderName, out var incomingSignature)
                || string.IsNullOrWhiteSpace(incomingSignature))
                return Unauthorized("HMAC: X-HMAC-SIGNATURE header missing");

            // 4. Rebuild raw string — METHOD|PATH|TIMESTAMP|BODY
            //    GET callers pass body="" (default); POST callers pass the actual request body.
            var method  = Request.Method.ToUpperInvariant();
            var rawData = $"{method}|{path}|{timestamp}|{body}";
            _logger.LogInformation("HMAC RAW STRING: {raw}", rawData);

            // 5. Compute expected signature
            var keyBytes      = Encoding.UTF8.GetBytes(HmacSecret);
            var messageBytes  = Encoding.UTF8.GetBytes(rawData);
            using var hmac    = new HMACSHA256(keyBytes);
            var hashBytes     = hmac.ComputeHash(messageBytes);
            var expectedSig   = Convert.ToBase64String(hashBytes);
            _logger.LogInformation("HMAC SIGNATURE: {signature}", expectedSig);

            // 6. Constant-time comparison to resist timing attacks
            if (!CryptographicOperations.FixedTimeEquals(
                    Encoding.UTF8.GetBytes(expectedSig),
                    Encoding.UTF8.GetBytes(incomingSignature.ToString())))
            {
                Console.Error.WriteLine(
                    $"[HMAC] Signature mismatch. Method={method} Path={path} Timestamp={timestamp}");
                return Unauthorized("HMAC: Signature mismatch");
            }

            return null; // valid
        }

        // ─────────────────────────────────────────────────────────────
        // Helpers
        // ─────────────────────────────────────────────────────────────
        private static string SampleApplicantName(int i) => i switch
        {
            1  => "Ramesh Kumar",
            2  => "Sunita Devi",
            3  => "Bimal Sen",
            4  => "Kavita Roy",
            5  => "Manoj Sharma",
            6  => "Puja Ghosh",
            7  => "Tapas Biswas",
            8  => "Rekha Haldar",
            9  => "Arun Pal",
            10 => "Mita Saha",
            _  => $"Applicant {i}"
        };

        private record OfficeRef(
            string Code, int LevelId, string Name, string DistrictName);

        private record ServiceRef(
            int Code, string Name, int StimulateDays);
    }

    // ─────────────────────────────────────────────────────────────────
    // Response wrapper
    // ─────────────────────────────────────────────────────────────────
    public class ApiResponse<T>
    {
    [JsonPropertyName("success")]    public bool     Success    { get; set; }
    [JsonPropertyName("message")]    public string   Message    { get; set; } = string.Empty;
    [JsonPropertyName("payload_id")] public string   PayloadId  { get; set; } = string.Empty;
    [JsonPropertyName("total_count")] public int      TotalCount { get; set; }
    [JsonPropertyName("data")]       public List<T>  Data       { get; set; } = [];

    public static ApiResponse<T> Ok(List<T> data, int totalCount, string msg, string payloadId) => new()
        { Success = true, Message = msg, TotalCount = totalCount, Data = data, PayloadId = payloadId };

    public static ApiResponse<T> Empty(string msg) => new()
        { Success = true, Message = msg, TotalCount = 0, Data = [], PayloadId = string.Empty };
}

    // ─────────────────────────────────────────────────────────────────
    // DTOs
    // ─────────────────────────────────────────────────────────────────

    public class OfficeDto
    {
        [JsonPropertyName("office_code")]      public string  office_code      { get; set; } = default!;
        [JsonPropertyName("office_name")]      public string  office_name      { get; set; } = default!;
        [JsonPropertyName("department_code")]  public string  department_code  { get; set; } = default!;
        [JsonPropertyName("department_id")]    public int     department_id    { get; set; }
        [JsonPropertyName("district_name")]    public string? district_name    { get; set; }
        [JsonPropertyName("state_name")]       public string? state_name       { get; set; }
        [JsonPropertyName("state_id")]         public int     state_id         { get; set; }
        [JsonPropertyName("level_id")]         public int     level_id         { get; set; }
        [JsonPropertyName("level_key")]        public string  level_key        { get; set; } = default!;
        [JsonPropertyName("parent_office_id")] public int?    parent_office_id { get; set; }
        [JsonPropertyName("is_active")]        public bool    is_active        { get; set; }
    }

    public class ServiceDto
    {
        [JsonPropertyName("service_code")]     public int     service_code     { get; set; }
        [JsonPropertyName("service_name")]     public string  service_name     { get; set; } = default!;
        [JsonPropertyName("department_code")]  public string  department_code  { get; set; } = default!;
        [JsonPropertyName("department_id")]    public int     department_id    { get; set; }
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

    public class UserDto
    {
        [JsonPropertyName("official_email")]  public string  official_email  { get; set; } = default!;
        [JsonPropertyName("full_name")]       public string  full_name       { get; set; } = default!;
        [JsonPropertyName("mobile_no")]       public string? mobile_no       { get; set; }
        [JsonPropertyName("designation")]     public string  designation     { get; set; } = default!;
        [JsonPropertyName("role_key")]        public string  role_key        { get; set; } = default!;
        [JsonPropertyName("office_code")]     public string  office_code     { get; set; } = default!;
        [JsonPropertyName("department_code")] public string  department_code { get; set; } = default!;
        [JsonPropertyName("is_active")]       public bool    is_active       { get; set; }
    }

    public class AcknowledgementDto
    {
        [JsonPropertyName("acknowledgement_no")] public string  acknowledgement_no { get; set; } = default!;
        [JsonPropertyName("application_no")]     public string? application_no     { get; set; }
        [JsonPropertyName("applicant_name")]     public string  applicant_name     { get; set; } = default!;
        [JsonPropertyName("applicant_mobile")]   public string? applicant_mobile   { get; set; }
        [JsonPropertyName("applicant_email")]    public string? applicant_email    { get; set; }
        [JsonPropertyName("service_code")]       public int     service_code       { get; set; }
        [JsonPropertyName("office_code")]        public string  office_code        { get; set; } = default!;
        [JsonPropertyName("official_email")]     public string? official_email     { get; set; }
        [JsonPropertyName("department_code")]    public string  department_code    { get; set; } = default!;
        [JsonPropertyName("present_status")]     public string  present_status     { get; set; } = default!;
        [JsonPropertyName("applied_date")]       public string? applied_date       { get; set; }
        [JsonPropertyName("last_updated_date")]  public string? last_updated_date  { get; set; }
    }

    public class CallbackPayload
    {
        [JsonPropertyName("payload_id")]    public string?              PayloadId    { get; set; }
        [JsonPropertyName("status")]        public string               Status       { get; set; } = string.Empty;
        [JsonPropertyName("total_records")] public int                  TotalRecords { get; set; }
        [JsonPropertyName("success_count")] public int                  SuccessCount { get; set; }
        [JsonPropertyName("failure_count")] public int                  FailureCount { get; set; }
        [JsonPropertyName("errors")]        public List<CallbackError>? Errors       { get; set; }
    }

    public class CallbackError
    {
        [JsonPropertyName("referenceId")]    public string ReferenceId   { get; set; } = string.Empty;
        [JsonPropertyName("errorMessage")]   public string ErrorMessage  { get; set; } = string.Empty;
    }
}