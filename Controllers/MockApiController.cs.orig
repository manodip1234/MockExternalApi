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
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // AUTH CONSTANTS
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        private const string ApiKeyHeaderName    = "X-API-KEY";
        private const string TimestampHeaderName = "X-TIMESTAMP";
        private const string SignatureHeaderName = "X-HMAC-SIGNATURE";

        // API_KEY auth constant (shared with HMAC for key validation)
        private const string ValidApiKey         = "rtps-demo-key-001";

        // HMAC signing secret
        private const string HmacSecret          = "rtps-demo-secret-001";

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // GROUND TRUTH ├óΓé¼ΓÇ¥ values that MUST exist in the DB before sync
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼

        // Department IDs are looked up by the ingestion service via department_code.
        // These values are only used for the mock API response payload ├óΓé¼ΓÇ¥ the
        // ingestion service ignores department_id from the payload and resolves
        // it from rtps_wb.department by department_code.
        private static readonly Dictionary<string, int> _deptMap = new()
        {
            ["BCW"]   = 4,
            ["FSD"]   = 1,
            ["FOOD"]  = 1,
            ["AGR"]   = 5,
            ["TRANS"] = 6,
            ["ENV"]   = 7,
            ["WRD"]   = 13,
        };

        // state_id=0 signals the ingestion service to resolve West Bengal's
        // real state_id from rtps_master.state via the fallback lookup.
        private const int WestBengalStateId = 0;

        private static readonly Dictionary<string, int> _levelKeyToId = new()
        {
            ["STATE"]        = 1,
            ["DISTRICT"]     = 2,
            ["BLOCK"]        = 3,
            ["MUNICIPALITY"] = 4,
        };

        private static readonly Dictionary<string, int> _districtNameToId = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Kolkata"] = 10,
            ["Howrah"] = 11,
            ["Bardhaman"] = 12,
            ["North 24 Parganas"] = 13,
            ["South 24 Parganas"] = 14,
            ["Nadia"] = 15,
            ["Murshidabad"] = 16,
            ["Birbhum"] = 17,
            ["Darjeeling"] = 18,
            ["Jalpaiguri"] = 19,
            ["Paschim Medinipur"] = 20,
            ["Bankura"] = 21,
            ["Purulia"] = 22,
            ["Asansol"] = 23,
            ["Siliguri"] = 24
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
            // BCW ΓÇö codes/names match rtps_wb.office (source=API rows, ids 45-48)
            new("OFF-BCW-2A21EDB28681", 1, "BCW State Office Kolkata",          "Kolkata"),
            new("OFF-BCW-26BF2BD85AAB", 2, "BCW District Office Kolkata",        "Kolkata"),
            new("OFF-BCW-E836B6B786C1", 2, "BCW District Office Bardhaman",      "Bardhaman"),
            new("OFF-BCW-64857254172B", 2, "BCW Block Office North 24 Parganas", "North 24 Parganas"),
            // FOOD - Expanded with fresh data
            new("OFF-FOOD-1",  2, "Food District Office Kolkata",           "Kolkata"),
            new("OFF-FOOD-2",  1, "Food State Office Kolkata",               "Kolkata"),
            new("OFF-FOOD-3",  2, "Food District Office Howrah",             "Howrah"),
            new("OFF-FOOD-4",  3, "Food Block Office Murshidabad",           "Murshidabad"),
            new("OFF-FOOD-5",  2, "Food District Office Bardhaman",          "Bardhaman"),
            new("OFF-FOOD-6",  3, "Food Block Office North 24 Parganas",     "North 24 Parganas"),
            new("OFF-FOOD-7",  2, "Food District Office Nadia",              "Nadia"),
            new("OFF-FOOD-8",  3, "Food Block Office Birbhum",               "Birbhum"),
            // AGR - Expanded with fresh data
            new("OFF-AGR-1",   2, "Agriculture District Office Kolkata",    "Kolkata"),
            new("OFF-AGR-2",   1, "Agriculture State Office Kolkata",        "Kolkata"),
            new("OFF-AGR-3",   2, "Agriculture District Office Nadia",       "Nadia"),
            new("OFF-AGR-4",   3, "Agriculture Block Office Birbhum",        "Birbhum"),
            new("OFF-AGR-5",   2, "Agriculture District Office Howrah",      "Howrah"),
            new("OFF-AGR-6",   3, "Agriculture Block Office Murshidabad",    "Murshidabad"),
            new("OFF-AGR-7",   2, "Agriculture District Office Bardhaman",   "Bardhaman"),
            new("OFF-AGR-8",   3, "Agriculture Block Office North 24 Parganas", "North 24 Parganas"),
            // ENV - Expanded with fresh data
            new("OFF-ENV-1",   2, "Environment District Office Kolkata",    "Kolkata"),
            new("OFF-ENV-2",   1, "Environment State Office Kolkata",        "Kolkata"),
            new("OFF-ENV-3",   2, "Environment District Office Darjeeling",  "Darjeeling"),
            new("OFF-ENV-4",   3, "Environment Block Office Jalpaiguri",     "Jalpaiguri"),
            new("OFF-ENV-5",   2, "Environment District Office Howrah",      "Howrah"),
            new("OFF-ENV-6",   3, "Environment Block Office Nadia",          "Nadia"),
            new("OFF-ENV-7",   2, "Environment District Office Bardhaman",   "Bardhaman"),
            new("OFF-ENV-8",   3, "Environment Block Office Birbhum",        "Birbhum"),
            // TRANS - Expanded with fresh data
            new("OFF-TRANS-1", 2, "Transport District Office Kolkata",      "Kolkata"),
            new("OFF-TRANS-2", 1, "Transport State Office Kolkata",          "Kolkata"),
            new("OFF-TRANS-3", 2, "Transport District Office Asansol",       "Asansol"),
            new("OFF-TRANS-4", 3, "Transport Block Office Siliguri",         "Siliguri"),
            new("OFF-TRANS-5", 2, "Transport District Office Howrah",        "Howrah"),
            new("OFF-TRANS-6", 3, "Transport Block Office Darjeeling",       "Darjeeling"),
            new("OFF-TRANS-7", 2, "Transport District Office Bardhaman",     "Bardhaman"),
            new("OFF-TRANS-8", 3, "Transport Block Office Nadia",            "Nadia"),
            // WRD - use district names that exist in rtps_master.district
            new("OFF-WRD-1",   2, "Water Resource District Office Kolkata",    "Kolkata"),
            new("OFF-WRD-2",   1, "Water Resource State Office Kolkata",        "Kolkata"),
            new("OFF-WRD-3",   2, "Water Resource District Office Paschim Medinipur", "Paschim Medinipur"),
            new("OFF-WRD-4",   3, "Water Resource Block Office Bankura",        "Bankura"),
            new("OFF-WRD-5",   2, "Water Resource District Office Howrah",      "Howrah"),
            new("OFF-WRD-6",   3, "Water Resource Block Office Purulia",        "Purulia"),
            new("OFF-WRD-7",   2, "Water Resource District Office Nadia",       "Nadia"),
            new("OFF-WRD-8",   3, "Water Resource Block Office Birbhum",        "Birbhum"),
        ];

        private static readonly ServiceRef[] _realServices =
        [
            // BCW ΓÇö service_name must match rtps_wb.service exactly for name-based promotion
            new(784217, "BCW Caste Certificate Service", 30),
            new(784218, "BCW Welfare Scheme Service",    45),
            new(784219, "BCW Scholarship Grant Service",  30),
            new(30003,  "BCW Community Certificate",     20),
            new(3001, "Food Service A",  15),
            new(3002, "Food Service B",  25),
            new(3003, "Food Service C",  35),
            new(3004, "Food Service D",  40),
            new(3005, "Food Service E",  18),
            new(3006, "Food Service F",  28),
            new(4001, "AGR Service A",   20),
            new(4002, "AGR Service B",   30),
            new(4003, "AGR Service C",   45),
            new(4004, "AGR Service D",   22),
            new(4005, "AGR Service E",   38),
            new(4006, "AGR Service F",   27),
            new(5001, "ENV Service A",   21),
            new(5002, "ENV Service B",   14),
            new(5003, "ENV Service C",   28),
            new(5004, "ENV Service D",   33),
            new(5005, "ENV Service E",   19),
            new(5006, "ENV Service F",   42),
            new(6001, "TRANS Service A", 10),
            new(6002, "TRANS Service B", 20),
            new(6003, "TRANS Service C", 30),
            new(6004, "TRANS Service D", 15),
            new(6005, "TRANS Service E", 25),
            new(6006, "TRANS Service F", 35),
            new(7001, "WRD Service A",   25),
            new(7002, "WRD Service B",   40),
            new(7003, "WRD Service C",   15),
            new(7004, "WRD Service D",   32),
            new(7005, "WRD Service E",   18),
            new(7006, "WRD Service F",   48),
        ];

        // Per-department office/service slices
        private static OfficeRef[] OfficesFor(string dept) => dept.ToUpper() switch
        {
            "FOOD" or "FSD" => _realOffices.Where(o => o.Code.StartsWith("OFF-FOOD")).ToArray(),
            "AGR"           => _realOffices.Where(o => o.Code.StartsWith("OFF-AGR")).ToArray(),
            "ENV"           => _realOffices.Where(o => o.Code.StartsWith("OFF-ENV")).ToArray(),
            "TRANS"         => _realOffices.Where(o => o.Code.StartsWith("OFF-TRANS")).ToArray(),
            "WRD"           => _realOffices.Where(o => o.Code.StartsWith("OFF-WRD")).ToArray(),
            _               => _realOffices.Where(o => o.Code.StartsWith("OFF-BCW")).ToArray(),
        };

        private static ServiceRef[] ServicesFor(string dept) => dept.ToUpper() switch
        {
            "FOOD" or "FSD" => _realServices.Where(s => s.Code >= 3001 && s.Code <= 3999).ToArray(),
            "AGR"           => _realServices.Where(s => s.Code >= 4001 && s.Code <= 4999).ToArray(),
            "ENV"           => _realServices.Where(s => s.Code >= 5001 && s.Code <= 5999).ToArray(),
            "TRANS"         => _realServices.Where(s => s.Code >= 6001 && s.Code <= 6999).ToArray(),
            "WRD"           => _realServices.Where(s => s.Code >= 7001 && s.Code <= 7999).ToArray(),
            _               => _realServices.Where(s => s.Code < 3000).ToArray(),
        };

        private static int? ResolveDistrictId(string? districtName)
            => districtName != null && _districtNameToId.TryGetValue(districtName, out var id) ? id : null;

        private static int? ResolveBlockId(string? districtName)
            => ResolveDistrictId(districtName) is int districtId ? districtId + 100 : null;

        private static bool IsCustomJurisdictionService(string dept, int serviceCode)
            => false; // no custom jurisdiction services in current BCW dataset

        private static List<JurisdictionDto> BuildServiceCoverageRules(string dept)
        {
            var first = OfficesFor(dept).FirstOrDefault();
            var second = OfficesFor(dept).Skip(1).FirstOrDefault();

            var rules = new List<JurisdictionDto>();

            if (first != null)
            {
                rules.Add(new JurisdictionDto
                {
                    state_name = "West Bengal",
                    district_name = first.DistrictName,
                    block_name = $"{first.DistrictName} Block"
                });
            }

            if (second != null)
            {
                rules.Add(new JurisdictionDto
                {
                    state_name = "West Bengal",
                    district_name = second.DistrictName,
                    municipality_name = second.DistrictName == "Howrah" ? "Howrah Municipality" : null,
                    ward_name = second.DistrictName == "Howrah" ? "Ward 1" : null
                });
            }

            return rules;
        }

        private readonly ILogger<MockApiController> _logger;

        public MockApiController(ILogger<MockApiController> logger)
        {
            _logger = logger;
        }

        private static string OfficerEmail(int n, string deptCode)
            => $"officer{n}.{deptCode.ToLower()}@wb.gov.in";

        private int ResolveDeptId(string deptCode)
            => _deptMap.TryGetValue(deptCode.ToUpper(), out var id) ? id : 4;

        // Stable payload_id ├óΓé¼ΓÇ¥ changes only once per day per pipeline
        private static string StablePayloadId(string dept, string type)
            => $"P-{dept.ToUpper()}-{type}-{DateTime.UtcNow:yyyyMMddHH}";

        // ACK numbers include date so each day produces new unique records
        private static string AckNo(string dept, int i)
            => $"ACK/{dept.ToUpper()}/{DateTime.UtcNow:yyyyMM}/{i:D5}";

        // Returns the paged slice; totalCount is always the full list size
        private static (List<T> Page, int TotalCount) Paginate<T>(List<T> all, int page, int pageSize)
        {
            var total  = all.Count;
            var paged  = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return (paged, total);
        }

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // OFFICE endpoint  ├óΓÇáΓÇÖ  auth: HMAC
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
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
            var normalizedDept = departmentCode.ToUpper() == "FOOD" ? "FSD" : departmentCode.ToUpper();
            var deptId = ResolveDeptId(normalizedDept);
            var all = OfficesFor(normalizedDept).Select(o => new OfficeDto
            {
                office_name      = o.Name,
                department_code  = normalizedDept,
                department_id    = deptId,
                district_name    = o.DistrictName,
                state_name       = "West Bengal",
                state_id         = WestBengalStateId,
                level_id         = o.LevelId,
                level_key        = _levelKeyToId.First(kv => kv.Value == o.LevelId).Key,
                parent_office_id = null,
                is_active        = true,
                jurisdiction_mode = "LGD",
                jurisdiction_scope = "OFFICE",
                jurisdiction = new JurisdictionDto
                {
                    state_name = "West Bengal",
                    district_name = o.DistrictName,
                    block_name = o.LevelId == 3 ? $"{o.DistrictName} Block" : null,
                    municipality_name = null
                }
            }).ToList();
            var (paged, total) = Paginate(all, page, pageSize);
            return Ok(ApiResponse<OfficeDto>.Ok(paged, total, "Office data", StablePayloadId(normalizedDept, "OFFICE")));
        }

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // SERVICE endpoint  ├óΓÇáΓÇÖ  auth: HMAC
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
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
            var normalizedDept = departmentCode.ToUpper() == "FOOD" ? "FSD" : departmentCode.ToUpper();
            var all = ServicesFor(normalizedDept).Select(s => new ServiceDto
            {
                service_name     = s.Name,
                department_code  = normalizedDept,
                department_id    = ResolveDeptId(normalizedDept),
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
                jurisdiction_mode  = IsCustomJurisdictionService(normalizedDept, s.Code) ? "CUSTOM" : "LGD",
                jurisdiction_scope = "SERVICE",
                jurisdiction = IsCustomJurisdictionService(normalizedDept, s.Code)
                    ? new JurisdictionDto
                    {
                        state_name = "West Bengal",
                        district_name = OfficesFor(normalizedDept).First().DistrictName,
                        block_name = $"{OfficesFor(normalizedDept).First().DistrictName} Block"
                    }
                    : null,
                // jurisdiction_rules = IsCustomJurisdictionService(normalizedDept, s.Code)
                //    ? BuildServiceCoverageRules(normalizedDept)
                //    : null,
                beyond_department_days = (s.Code % 2 == 0) ? 5 : null,
                has_external_dependency = (s.Code % 2 == 0)
            }).ToList();
            var (paged, total) = Paginate(all, page, pageSize);
            return Ok(ApiResponse<ServiceDto>.Ok(paged, total, "Service data", StablePayloadId(normalizedDept, "SERVICE")));
        }

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // USER endpoint  ├óΓÇáΓÇÖ  auth: HMAC
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
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
            var normalizedDept = departmentCode.ToUpper() == "FOOD" ? "FSD" : departmentCode.ToUpper();

            if (normalizedDept.Equals("EMPTY", StringComparison.OrdinalIgnoreCase))
                return Ok(ApiResponse<UserDto>.Empty("No users found"));

            var deptOffices = OfficesFor(normalizedDept);
            var all = new List<UserDto>
            {
                new() { official_email = OfficerEmail(1, normalizedDept), full_name = "Ayan Chakraborty", mobile_no = "9800000001", designation = "Additional Chief Secretary", role_key = "DESIGNATED_OFFICER", office_name = deptOffices[0].Name,                         department_code = normalizedDept, is_active = true },
                new() { official_email = OfficerEmail(2, normalizedDept), full_name = "Priya Banerjee",   mobile_no = "9800000002", designation = "Principal Secretary",        role_key = "APPELLATE_OFFICER",  office_name = deptOffices[1 % deptOffices.Length].Name,  department_code = normalizedDept, is_active = true },
                new() { official_email = OfficerEmail(3, normalizedDept), full_name = "Suresh Mondal",    mobile_no = "9800000003", designation = "Secretary",                  role_key = "REVIEWING_OFFICER",  office_name = deptOffices[2 % deptOffices.Length].Name,  department_code = normalizedDept, is_active = true },
                new() { official_email = OfficerEmail(4, normalizedDept), full_name = "Rina Das",         mobile_no = "9800000004", designation = "Deputy Secretary",           role_key = "DESIGNATED_OFFICER", office_name = deptOffices[3 % deptOffices.Length].Name,  department_code = normalizedDept, is_active = true },
                new() { official_email = OfficerEmail(5, normalizedDept), full_name = "Karan Singh",      mobile_no = "9800000005", designation = "Joint Secretary",            role_key = "APPELLATE_OFFICER",  office_name = deptOffices[4 % deptOffices.Length].Name,  department_code = normalizedDept, is_active = true },
                new() { official_email = OfficerEmail(6, normalizedDept), full_name = "Meera Patel",      mobile_no = "9800000006", designation = "Assistant Secretary",        role_key = "REVIEWING_OFFICER",  office_name = deptOffices[5 % deptOffices.Length].Name,  department_code = normalizedDept, is_active = true },
            };
            var (paged, total) = Paginate(all, page, pageSize);
            return Ok(ApiResponse<UserDto>.Ok(paged, total, "User data", StablePayloadId(normalizedDept, "USER")));
        }

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // ACKNOWLEDGEMENT endpoint  ├óΓÇáΓÇÖ  auth: HMAC
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
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
            var normalizedDept = departmentCode.ToUpper() == "FOOD" ? "FSD" : departmentCode.ToUpper();

            if (normalizedDept.Equals("EMPTY", StringComparison.OrdinalIgnoreCase))
                return Ok(ApiResponse<AcknowledgementDto>.Empty("No acknowledgements found"));

            var statuses     = new[] { "IN_PROGRESS", "DISPOSED", "REJECTED", "PENDING" };
            var deptOffices  = OfficesFor(normalizedDept);
            var deptServices = ServicesFor(normalizedDept);
            var all = Enumerable.Range(1, 20).Select(i =>
            {
                var svc = deptServices[(i - 1) % deptServices.Length];
                var off = deptOffices[(i - 1)  % deptOffices.Length];
                return new AcknowledgementDto
                {
                    acknowledgement_no = AckNo(normalizedDept, i),
                    application_no     = $"APP/{normalizedDept}/{DateTime.UtcNow:yyyyMM}/{i:D5}",
                    applicant_name     = SampleApplicantName(i),
                    applicant_mobile   = $"980000{i:D4}",
                    applicant_email    = $"citizen.{normalizedDept.ToLower()}{i}@example.com",
                    service_name       = svc.Name,
                    office_name        = off.Name,
                    official_email     = OfficerEmail((i % 6) + 1, normalizedDept),
                    department_code    = normalizedDept,
                    present_status     = statuses[(i - 1) % statuses.Length],
                    applied_date       = DateTime.UtcNow.AddDays(-(i * 5)).ToString("yyyy-MM-dd"),
                    last_updated_date  = DateTime.UtcNow.AddDays(-(i * 2)).ToString("yyyy-MM-dd"),
                    NumberOfDaysBeyondDepartmentScope = (i % 3 == 0) ? 3 : null,
                };
            }).ToList();
            var (paged, total) = Paginate(all, page, pageSize);
            return Ok(ApiResponse<AcknowledgementDto>.Ok(paged, total, "Acknowledgement data", StablePayloadId(normalizedDept, "ACK")));
        }

        // ├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É
        // DEPARTMENT-SPECIFIC ENDPOINTS
        // ├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // BCW DEPARTMENT ENDPOINTS
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        [HttpGet("bcw/office")]
        public IActionResult GetBcwOffice([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[BCW-OFFICE] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildOfficeResponse("BCW", page, page_size);
        }

        [HttpGet("bcw/service")]
        public IActionResult GetBcwService([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[BCW-SERVICE] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildServiceResponse("BCW", page, page_size);
        }

        [HttpGet("bcw/user")]
        public IActionResult GetBcwUser([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[BCW-USER] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildUserResponse("BCW", page, page_size);
        }

        [HttpGet("bcw/acknowledgement")]
        [HttpGet("bcw/ack")]
        public IActionResult GetBcwAcknowledgement([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[BCW-ACK] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildAckResponse("BCW", page, page_size);
        }

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // FOOD DEPARTMENT ENDPOINTS
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        [HttpGet("food/office")]
        public IActionResult GetFoodOffice([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[FOOD-OFFICE] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildOfficeResponse("FOOD", page, page_size);
        }

        [HttpGet("food/service")]
        public IActionResult GetFoodService([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[FOOD-SERVICE] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildServiceResponse("FOOD", page, page_size);
        }

        [HttpGet("food/user")]
        public IActionResult GetFoodUser([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[FOOD-USER] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildUserResponse("FOOD", page, page_size);
        }

        [HttpGet("food/acknowledgement")]
        [HttpGet("food/ack")]
        public IActionResult GetFoodAcknowledgement([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[FOOD-ACK] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildAckResponse("FOOD", page, page_size);
        }

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // AGR DEPARTMENT ENDPOINTS
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        [HttpGet("agr/office")]
        public IActionResult GetAgrOffice([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[AGR-OFFICE] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildOfficeResponse("AGR", page, page_size);
        }

        [HttpGet("agr/service")]
        public IActionResult GetAgrService([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[AGR-SERVICE] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildServiceResponse("AGR", page, page_size);
        }

        [HttpGet("agr/user")]
        public IActionResult GetAgrUser([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[AGR-USER] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildUserResponse("AGR", page, page_size);
        }

        [HttpGet("agr/acknowledgement")]
        [HttpGet("agr/ack")]
        public IActionResult GetAgrAcknowledgement([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[AGR-ACK] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);
            return BuildAckResponse("AGR", page, page_size);
        }

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // ENV  ├óΓé¼ΓÇ¥  SCENARIO: Happy path (all valid, all FKs resolvable)
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        [HttpGet("env/office")]
        public IActionResult GetEnvOffice([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[ENV-OFFICE] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<OfficeDto>
            {
                new() { office_name = "Environment District Office Kolkata",    department_code = "ENV", department_id = 7, district_name = "Kolkata",    state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_name = "Environment State Office Kolkata",      department_code = "ENV", department_id = 7, district_name = "Kolkata",    state_name = "West Bengal", state_id = WestBengalStateId, level_id = 1, level_key = "STATE",    is_active = true },
                new() { office_name = "Environment District Office Darjeeling",department_code = "ENV", department_id = 7, district_name = "Darjeeling", state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_name = "Environment Block Office Jalpaiguri",   department_code = "ENV", department_id = 7, district_name = "Jalpaiguri", state_name = "West Bengal", state_id = WestBengalStateId, level_id = 3, level_key = "BLOCK",    is_active = true },
                new() { office_name = "Environment District Office Howrah",    department_code = "ENV", department_id = 7, district_name = "Howrah",     state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_name = "Environment Block Office Nadia",        department_code = "ENV", department_id = 7, district_name = "Nadia",      state_name = "West Bengal", state_id = WestBengalStateId, level_id = 3, level_key = "BLOCK",    is_active = true },
                new() { office_name = "Environment District Office Bardhaman", department_code = "ENV", department_id = 7, district_name = "Bardhaman",  state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_name = "Environment Block Office Birbhum",      department_code = "ENV", department_id = 7, district_name = "Birbhum",    state_name = "West Bengal", state_id = WestBengalStateId, level_id = 3, level_key = "BLOCK",    is_active = true },
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
            _logger.LogInformation("[ENV-SERVICE] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<ServiceDto>
            {
                new() { service_name = "ENV Service A", department_code = "ENV", department_id = 7, stipulated_days = 21, resolution_days = 26, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "ENV Service B", department_code = "ENV", department_id = 7, stipulated_days = 14, resolution_days = 19, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "ENV Service C", department_code = "ENV", department_id = 7, stipulated_days = 28, resolution_days = 33, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "ENV Service D", department_code = "ENV", department_id = 7, stipulated_days = 33, resolution_days = 38, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "ENV Service E", department_code = "ENV", department_id = 7, stipulated_days = 19, resolution_days = 24, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "ENV Service F", department_code = "ENV", department_id = 7, stipulated_days = 42, resolution_days = 47, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
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
            _logger.LogInformation("[ENV-USER] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<UserDto>
            {
                new() { official_email = "officer1.env@wb.gov.in", full_name = "Ananya Das",       mobile_no = "9811000001", designation = "Additional Chief Secretary", role_key = "DESIGNATED_OFFICER", department_code = "ENV", is_active = true },
                new() { official_email = "officer2.env@wb.gov.in", full_name = "Debashis Roy",     mobile_no = "9811000002", designation = "Principal Secretary",        role_key = "APPELLATE_OFFICER",  department_code = "ENV", is_active = true },
                new() { official_email = "officer3.env@wb.gov.in", full_name = "Mitali Ghosh",     mobile_no = "9811000003", designation = "Secretary",                  role_key = "REVIEWING_OFFICER",  department_code = "ENV", is_active = true },
                new() { official_email = "officer4.env@wb.gov.in", full_name = "Rajib Chatterjee", mobile_no = "9811000004", designation = "Deputy Secretary",           role_key = "DESIGNATED_OFFICER", department_code = "ENV", is_active = true },
                new() { official_email = "officer5.env@wb.gov.in", full_name = "Smita Banerjee",   mobile_no = "9811000005", designation = "Joint Secretary",            role_key = "APPELLATE_OFFICER",  department_code = "ENV", is_active = true },
                new() { official_email = "officer6.env@wb.gov.in", full_name = "Arindam Pal",      mobile_no = "9811000006", designation = "Assistant Secretary",        role_key = "REVIEWING_OFFICER",  department_code = "ENV", is_active = true },
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
            _logger.LogInformation("[ENV-ACK] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var statuses = new[] { "DISPOSED", "IN_PROGRESS", "PENDING", "REJECTED", "DISPOSED", "IN_PROGRESS" };
            var all = Enumerable.Range(1, 15).Select(i => new AcknowledgementDto
            {
                acknowledgement_no = AckNo("ENV", i),
                application_no     = $"APP/ENV/{DateTime.UtcNow:yyyyMM}/{i:D5}",
                applicant_name     = SampleApplicantName(i),
                applicant_mobile   = $"9811100{i:D3}",
                applicant_email    = $"citizen.env{i}@example.com",
                service_name = $"Service {(char)('A' + ((i - 1) % 6))}",
                office_name = $"Office {((i - 1) % 8) + 1}",
                official_email     = $"officer{((i % 6) + 1)}.env@wb.gov.in",
                department_code    = "ENV",
                present_status     = statuses[(i - 1) % statuses.Length],
                applied_date       = DateTime.UtcNow.AddDays(-(i * 3)).ToString("yyyy-MM-dd"),
                last_updated_date  = DateTime.UtcNow.AddDays(-i).ToString("yyyy-MM-dd"),
            }).ToList();
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<AcknowledgementDto>.Ok(paged, total, "Acknowledgement data", StablePayloadId("ENV", "ACK")));
        }

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // TRANS  ├óΓé¼ΓÇ¥  SCENARIO: Partial FK failure
        //   Offices 3 & 4 use codes that do NOT exist in rtps_wb.office
        //   ACK records 5-8 reference those ghost offices ├óΓÇáΓÇÖ retry queue
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        [HttpGet("trans/office")]
        public IActionResult GetTransOffice([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[TRANS-OFFICE] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<OfficeDto>
            {
                new() { office_name = "Transport District Office Kolkata",   department_code = "TRANS", department_id = 8, district_name = "Kolkata",    state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_name = "Transport State Office Kolkata",     department_code = "TRANS", department_id = 8, district_name = "Kolkata",    state_name = "West Bengal", state_id = WestBengalStateId, level_id = 1, level_key = "STATE",    is_active = true },
                new() { office_name = "Transport District Office Asansol",  department_code = "TRANS", department_id = 8, district_name = "Asansol",    state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_name = "Transport Block Office Siliguri",    department_code = "TRANS", department_id = 8, district_name = "Siliguri",   state_name = "West Bengal", state_id = WestBengalStateId, level_id = 3, level_key = "BLOCK",    is_active = true },
                new() { office_name = "Transport District Office Howrah",   department_code = "TRANS", department_id = 8, district_name = "Howrah",     state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_name = "Transport Block Office Darjeeling",  department_code = "TRANS", department_id = 8, district_name = "Darjeeling", state_name = "West Bengal", state_id = WestBengalStateId, level_id = 3, level_key = "BLOCK",    is_active = true },
                new() { office_name = "Transport District Office Bardhaman",department_code = "TRANS", department_id = 8, district_name = "Bardhaman",  state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_name = "Transport Block Office Nadia",       department_code = "TRANS", department_id = 8, district_name = "Nadia",      state_name = "West Bengal", state_id = WestBengalStateId, level_id = 3, level_key = "BLOCK",    is_active = true },
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
            _logger.LogInformation("[TRANS-SERVICE] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<ServiceDto>
            {
                new() { service_name = "TRANS Service A", department_code = "TRANS", department_id = 8, stipulated_days = 10, resolution_days = 15, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "TRANS Service B", department_code = "TRANS", department_id = 8, stipulated_days = 20, resolution_days = 25, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "TRANS Service C", department_code = "TRANS", department_id = 8, stipulated_days = 30, resolution_days = 35, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "TRANS Service D", department_code = "TRANS", department_id = 8, stipulated_days = 15, resolution_days = 20, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "TRANS Service E", department_code = "TRANS", department_id = 8, stipulated_days = 25, resolution_days = 30, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "TRANS Service F", department_code = "TRANS", department_id = 8, stipulated_days = 35, resolution_days = 40, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
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
            _logger.LogInformation("[TRANS-USER] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<UserDto>
            {
                new() { official_email = "officer1.trans@wb.gov.in", full_name = "Rajesh Sinha",    mobile_no = "9822000001", designation = "Additional Chief Secretary", role_key = "DESIGNATED_OFFICER", department_code = "TRANS", is_active = true },
                new() { official_email = "officer2.trans@wb.gov.in", full_name = "Soma Chatterjee", mobile_no = "9822000002", designation = "Principal Secretary",        role_key = "APPELLATE_OFFICER",  department_code = "TRANS", is_active = true },
                new() { official_email = "officer3.trans@wb.gov.in", full_name = "Nikhil Bose",     mobile_no = "9822000003", designation = "Secretary",                  role_key = "REVIEWING_OFFICER",  department_code = "TRANS", is_active = true },
                new() { official_email = "officer4.trans@wb.gov.in", full_name = "Priya Sharma",    mobile_no = "9822000004", designation = "Deputy Secretary",           role_key = "DESIGNATED_OFFICER", department_code = "TRANS", is_active = true },
                new() { official_email = "officer5.trans@wb.gov.in", full_name = "Amit Kumar",      mobile_no = "9822000005", designation = "Joint Secretary",            role_key = "APPELLATE_OFFICER",  department_code = "TRANS", is_active = true },
                new() { official_email = "officer6.trans@wb.gov.in", full_name = "Kavita Jain",     mobile_no = "9822000006", designation = "Assistant Secretary",        role_key = "REVIEWING_OFFICER",  department_code = "TRANS", is_active = true },
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
            _logger.LogInformation("[TRANS-ACK] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = Enumerable.Range(1, 20).Select(i => new AcknowledgementDto
            {
                acknowledgement_no = AckNo("TRANS", i),
                application_no     = $"APP/TRANS/{DateTime.UtcNow:yyyyMM}/{i:D5}",
                applicant_name     = SampleApplicantName(i),
                applicant_mobile   = $"9822200{i:D3}",
                applicant_email    = $"citizen.trans{i}@example.com",
                service_name = $"Service {(char)('A' + ((i - 1) % 6))}",
                office_name = $"Office {((i - 1) % 8) + 1}",
                official_email     = $"officer{((i % 6) + 1)}.trans@wb.gov.in",
                department_code    = "TRANS",
                present_status     = i % 2 == 0 ? "DISPOSED" : "IN_PROGRESS",
                applied_date       = DateTime.UtcNow.AddDays(-(i * 4)).ToString("yyyy-MM-dd"),
                last_updated_date  = DateTime.UtcNow.AddDays(-(i * 2)).ToString("yyyy-MM-dd"),
            }).ToList();
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<AcknowledgementDto>.Ok(paged, total, "Acknowledgement data", StablePayloadId("TRANS", "ACK")));
        }

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // WRD  ├óΓé¼ΓÇ¥  SCENARIO: Edge cases
        //   USER ├óΓÇáΓÇÖ empty list (tests empty-dataset handling)
        //   ACK  ├óΓÇáΓÇÖ 25 records (tests pagination); records 21-25 reuse
        //          ack numbers from 1-5 (tests duplicate handling)
        //   ACK  ├óΓÇáΓÇÖ null official_email (tests null FK handling)
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        [HttpGet("wrd/office")]
        public IActionResult GetWrdOffice([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[WRD-OFFICE] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<OfficeDto>
            {
                new() { office_name = "Water Resource District Office Kolkata",   department_code = "WRD", department_id = 13, district_name = "Kolkata",   state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_name = "Water Resource State Office Kolkata",     department_code = "WRD", department_id = 13, district_name = "Kolkata",   state_name = "West Bengal", state_id = WestBengalStateId, level_id = 1, level_key = "STATE",    is_active = true },
                new() { office_name = "Water Resource District Office Paschim Medinipur",department_code = "WRD", department_id = 13, district_name = "Paschim Medinipur", state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_name = "Water Resource Block Office Bankura",     department_code = "WRD", department_id = 13, district_name = "Bankura",   state_name = "West Bengal", state_id = WestBengalStateId, level_id = 3, level_key = "BLOCK",    is_active = true },
                new() { office_name = "Water Resource District Office Howrah",   department_code = "WRD", department_id = 13, district_name = "Howrah",    state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_name = "Water Resource Block Office Purulia",     department_code = "WRD", department_id = 13, district_name = "Purulia",   state_name = "West Bengal", state_id = WestBengalStateId, level_id = 3, level_key = "BLOCK",    is_active = true },
                new() { office_name = "Water Resource District Office Nadia",    department_code = "WRD", department_id = 13, district_name = "Nadia",     state_name = "West Bengal", state_id = WestBengalStateId, level_id = 2, level_key = "DISTRICT", is_active = true },
                new() { office_name = "Water Resource Block Office Birbhum",     department_code = "WRD", department_id = 13, district_name = "Birbhum",   state_name = "West Bengal", state_id = WestBengalStateId, level_id = 3, level_key = "BLOCK",    is_active = true },
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
            _logger.LogInformation("[WRD-SERVICE] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<ServiceDto>
            {
                new() { service_name = "WRD Service A", department_code = "WRD", department_id = 13, stipulated_days = 25, resolution_days = 30, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "WRD Service B", department_code = "WRD", department_id = 13, stipulated_days = 40, resolution_days = 45, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "WRD Service C", department_code = "WRD", department_id = 13, stipulated_days = 15, resolution_days = 20, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "WRD Service D", department_code = "WRD", department_id = 13, stipulated_days = 32, resolution_days = 37, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "WRD Service E", department_code = "WRD", department_id = 13, stipulated_days = 18, resolution_days = 23, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
                new() { service_name = "WRD Service F", department_code = "WRD", department_id = 13, stipulated_days = 48, resolution_days = 53, appeal_days = 30, reappeal_days = 60, stipulated_text = "Standard processing", is_active = true },
            };
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<ServiceDto>.Ok(paged, total, "Service data", StablePayloadId("WRD", "SERVICE")));
        }

        [HttpGet("wrd/user")]
        public IActionResult GetWrdUser([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[WRD-USER] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var all = new List<UserDto>
            {
                new() { official_email = "officer1.wrd@wb.gov.in", full_name = "Subrata Ghosh",   mobile_no = "9833000001", designation = "Additional Chief Secretary", role_key = "DESIGNATED_OFFICER", department_code = "WRD", is_active = true },
                new() { official_email = "officer2.wrd@wb.gov.in", full_name = "Tanmoy Basu",     mobile_no = "9833000002", designation = "Principal Secretary",        role_key = "APPELLATE_OFFICER",  department_code = "WRD", is_active = true },
                new() { official_email = "officer3.wrd@wb.gov.in", full_name = "Lipika Sen",      mobile_no = "9833000003", designation = "Secretary",                  role_key = "REVIEWING_OFFICER",  department_code = "WRD", is_active = true },
                new() { official_email = "officer4.wrd@wb.gov.in", full_name = "Partha Sarkar",   mobile_no = "9833000004", designation = "Deputy Secretary",           role_key = "DESIGNATED_OFFICER", department_code = "WRD", is_active = true },
                new() { official_email = "officer5.wrd@wb.gov.in", full_name = "Debjani Mitra",   mobile_no = "9833000005", designation = "Joint Secretary",            role_key = "APPELLATE_OFFICER",  department_code = "WRD", is_active = true },
                new() { official_email = "officer6.wrd@wb.gov.in", full_name = "Suman Chakraborty",mobile_no = "9833000006", designation = "Assistant Secretary",        role_key = "REVIEWING_OFFICER",  department_code = "WRD", is_active = true },
            };
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<UserDto>.Ok(paged, total, "User data", StablePayloadId("WRD", "USER")));
        }

        [HttpGet("wrd/acknowledgement")]
        [HttpGet("wrd/ack")]
        public IActionResult GetWrdAcknowledgement([FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;
            _logger.LogInformation("[WRD-ACK] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize} | Path={Path}", page, page_size, path);

            var statuses = new[] { "IN_PROGRESS", "DISPOSED", "REJECTED", "PENDING" };
            var all = Enumerable.Range(1, 20).Select(i => new AcknowledgementDto
            {
                acknowledgement_no = AckNo("WRD", i),
                application_no     = $"APP/WRD/{DateTime.UtcNow:yyyyMM}/{i:D5}",
                applicant_name     = SampleApplicantName((i - 1) % 10 + 1),
                applicant_mobile   = $"9833300{i:D3}",
                applicant_email    = $"citizen.wrd{i}@example.com",
                service_name = $"Service {(char)('A' + ((i - 1) % 6))}",
                office_name = $"Office {((i - 1) % 8) + 1}",
                official_email     = $"officer{((i % 6) + 1)}.wrd@wb.gov.in",
                department_code    = "WRD",
                present_status     = statuses[(i - 1) % statuses.Length],
                applied_date       = DateTime.UtcNow.AddDays(-(i * 2)).ToString("yyyy-MM-dd"),
                last_updated_date  = DateTime.UtcNow.AddDays(-i).ToString("yyyy-MM-dd"),
            }).ToList();
            var (paged, total) = Paginate(all, page, page_size);
            return Ok(ApiResponse<AcknowledgementDto>.Ok(paged, total, "Acknowledgement data", StablePayloadId("WRD", "ACK")));
        }

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // ABC SUBMISSIONS endpoint  ├óΓÇáΓÇÖ  Monthly MIS aggregate (Form A shape)
        // Route: GET /mock-api/abc/submissions
        // Returns monthly MIS summary records per service per office.
        // Same shape as Form A portal submissions ├óΓé¼ΓÇ¥ dept_code, office_id,
        // service_id, period_month, period_year, counts.
        // AbcSyncService calls this endpoint, groups records by
        // (dept_code, period_month, period_year), and promotes via
        // IApplicationRecordService.AddBatchAsync to rtps_wb.form_a_submission.
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        [HttpGet("abc/submissions")]
        public IActionResult GetAbcSubmissions(
            [FromQuery] int page = 1, [FromQuery] int page_size = 100)
        {
            var path       = Request.Path + Request.QueryString;
            var authResult = ValidateRequest("HMAC", path);
            if (authResult != null) return authResult;

            _logger.LogInformation(
                "[BCW-ABC-SYNC] ├ó┼ôΓÇ£ Request passed | Page={Page} PageSize={PageSize}",
                page, page_size);

            return BuildAbcSubmissionsResponse(page, page_size);
        }

        private IActionResult BuildAbcSubmissionsResponse(int page, int pageSize)
        {
            const string AbcDeptCode = "BCW";
            const string AbcDeptName = "Backward Classes Welfare Department";
            // index ΓåÆ form_type: 0,1 = A  |  2 = B  |  3 = C
            var fixtures = new[]
            {
                // Names must exactly match rtps_wb.service.service_name for promotion
                new { Form = "A", Service = "OBC Certificate Issuance", Office = "BCW District Office Bankura", Officer = "dwo.bnk.bcw@wb.gov.in" },
                new { Form = "A", Service = "SC Certificate Issuance", Office = "BCW District Office Birbhum", Officer = "dwo.brb.bcw@wb.gov.in" },
                new { Form = "B", Service = "Minority Welfare Scholarship", Office = "BCW District Office Murshidabad", Officer = "dwo.mur.bcw@wb.gov.in" },
                new { Form = "C", Service = "Pre-Matric Scholarship for OBC Students", Office = "BCW District Office Purulia", Officer = "dwo.prl.bcw@wb.gov.in" }
            };

            var all = new List<AbcSubmissionMisDto>();

            foreach (var (month, year) in new[] { (5, 2026), (6, 2026) })
            {
                foreach (var (fixture, idx) in fixtures.Select((value, index) => (value, index)))
                {
                    var seed     = (year * 100) + (month * 10) + idx + 1;
                    var received = 50 + (seed % 50);
                    var form1    = received - (seed % 10);
                    var within   = form1 - (seed % 8);
                    var after    = seed % 5;
                    var pending  = received - within - after;

                    all.Add(new AbcSubmissionMisDto
                    {
                        external_record_id    = $"BCW-{fixture.Form}-{year:D4}{month:D2}-{idx + 1:D2}",
                        dept_code             = AbcDeptCode,
                        department_code       = AbcDeptCode,
                        dept_name             = AbcDeptName,
                        department_name       = AbcDeptName,
                        office_name           = fixture.Office,
                        service_name          = fixture.Service,
                        form_type             = fixture.Form,
                        officer_email         = fixture.Officer,
                        period_month          = month,
                        period_year           = year,
                        applications_received = received,
                        form1_issued          = form1,
                        disposed_within_time  = within,
                        disposed_after_time   = after,
                        pending_applications  = pending < 0 ? 0 : pending,
                        appeals_received      = fixture.Form == "A" ? 0 : received,
                        pending_appeals       = fixture.Form == "A" ? 0 : Math.Max(0, received - within - after),
                        external_updated_at   = new DateTimeOffset(year, month, 28, 12, 0, 0, TimeSpan.Zero)
                    });
                }
            }

            var (paged, total) = Paginate(all, page, pageSize);
            return Ok(ApiResponse<AbcSubmissionMisDto>.Ok(
                paged, total,
                "ABC monthly MIS submission data",
                StablePayloadId("ABC", "SUBMISSIONS")));
        }

        // ABC callback receiver
        [HttpPost("abc/sync-response")]
        public Task<IActionResult> AbcCallback() => ReceiveSyncCallback();

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // ABC SUBMISSIONS endpoint  ├óΓÇáΓÇÖ  Monthly MIS aggregate (Form A shape)
        // Route: GET /mock-api/abc/submissions
        //
        // Returns monthly MIS summary records per service per period.
        // AbcSyncService fetches these pages, groups by
        // (dept_code, period_month, period_year), maps to
        // FormSubmissionBatchDto and calls AddBatchAsync ├óΓé¼ΓÇ¥
        // same pipeline as Form A portal submissions.
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
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

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // CALLBACK receiver  ├óΓÇáΓÇÖ  POST /mock-api/sync-response (shared)
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
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

            _logger.LogInformation("├óΓÇóΓÇ¥├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇóΓÇö");
            _logger.LogInformation("├óΓÇóΓÇÿ           [CALLBACK-RECEIVED] INCOMING POST              ├óΓÇóΓÇÿ");
            _logger.LogInformation("├óΓÇó┼í├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬É├óΓÇó┬¥");
            _logger.LogInformation("[CALLBACK-RECEIVED] Path      : {Path}", path);
            _logger.LogInformation("[CALLBACK-RECEIVED] X-API-KEY : {Key}",  apiKey);
            _logger.LogInformation("[CALLBACK-RECEIVED] X-TIMESTAMP: {Ts}",  timestamp);
            _logger.LogInformation("[CALLBACK-RECEIVED] X-HMAC-SIGNATURE: {Sig}", incomingSig);
            _logger.LogInformation("[CALLBACK-RECEIVED] Raw Body  : {Body}", rawBody);

            // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼ HMAC Validation ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
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
                _logger.LogWarning("[CALLBACK-REJECTED] ├ó┼ôΓÇö HMAC signature mismatch | Path={Path} | Timestamp={Ts}", path, timestamp);
                return Unauthorized("CALLBACK: Signature mismatch");
            }

            _logger.LogInformation("[CALLBACK-RECEIVED] ├ó┼ôΓÇ£ HMAC signature VALID");

            // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼ Deserialize payload ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
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

            // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼ Extract pipeline name from payload_id ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
            // Format: P-BCW-OFFICE-20260408095646  ├óΓÇáΓÇÖ  pipeline = OFFICE
            // UUID format (BCW): 7c99b30f-3385-4ca2-...  ├óΓÇáΓÇÖ  pipeline = UNKNOWN
            var pipeline = "UNKNOWN";
            if (!string.IsNullOrWhiteSpace(payload.PayloadId) && payload.PayloadId.StartsWith("P-"))
            {
                var parts = payload.PayloadId.Split('-');
                if (parts.Length >= 3) pipeline = parts[2];
            }

            // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼ Structured log summary ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
            _logger.LogInformation("├óΓÇ¥┼Æ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥┬É");
            _logger.LogInformation("├óΓÇ¥ΓÇÜ              [CALLBACK-RECEIVED] SUMMARY                 ├óΓÇ¥ΓÇÜ");
            _logger.LogInformation("├óΓÇ¥┼ô├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥┬ñ");
            _logger.LogInformation("├óΓÇ¥ΓÇÜ Pipeline   : {Pipeline,-52} ├óΓÇ¥ΓÇÜ", pipeline);
            _logger.LogInformation("├óΓÇ¥ΓÇÜ PayloadId  : {Id,-52} ├óΓÇ¥ΓÇÜ", payload.PayloadId ?? "[NULL]");
            _logger.LogInformation("├óΓÇ¥ΓÇÜ Status     : {Status,-52} ├óΓÇ¥ΓÇÜ", payload.Status);
            _logger.LogInformation("├óΓÇ¥ΓÇÜ Total      : {Total,-52} ├óΓÇ¥ΓÇÜ", payload.TotalRecords);
            _logger.LogInformation("├óΓÇ¥ΓÇÜ Success    : {Success,-52} ├óΓÇ¥ΓÇÜ", payload.SuccessCount);
            _logger.LogInformation("├óΓÇ¥ΓÇÜ Failed     : {Failed,-52} ├óΓÇ¥ΓÇÜ", payload.FailureCount);
            _logger.LogInformation("├óΓÇ¥ΓÇ¥├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥╦£");

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

            // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼ Pretty print to console ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
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
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // AUTH: UNIFIED DISPATCHER
        //
        // authType | behaviour
        // ---------|--------------------------------------------------
        // "NONE"   | Always passes ├óΓé¼ΓÇ¥ returns null
        // "API_KEY"| Delegates to ValidateApiKey()
        // "HMAC"   | Delegates to ValidateHmac(path)
        // other    | Returns 401 Unsupported auth type
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        private IActionResult? ValidateRequest(string authType, string path) =>
            authType.ToUpperInvariant() switch
            {
                "NONE"    => null,
                "API_KEY" => ValidateApiKey(),
                "HMAC"    => ValidateHmac(path),
                _         => Unauthorized($"Unsupported auth type: {authType}"),
            };

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // AUTH: API KEY
        //
        // Reads header X-API-KEY and compares against ValidApiKey.
        // Returns null on success, Unauthorized on failure.
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        private IActionResult? ValidateApiKey()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var key))
                return Unauthorized("API Key missing");

            if (key != ValidApiKey)
                return Unauthorized("Invalid API Key");

            return null; // valid
        }

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // AUTH: HMAC SHA-256
        //
        // Expected headers:
        //   X-API-KEY        ├óΓé¼ΓÇ¥ must equal ValidApiKey
        //   X-TIMESTAMP      ├óΓé¼ΓÇ¥ ISO / epoch string included in raw string
        //   X-HMAC-SIGNATURE ├óΓé¼ΓÇ¥ Base64(HMAC-SHA256(raw, HmacSecret))
        //
        // Raw string format (pipe-separated, no spaces around pipe):
        //   METHOD|PATH_AND_QUERY|TIMESTAMP|BODY
        //
        // For GET requests BODY is always "{}".
        // Returns null on success, Unauthorized on failure.
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
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

            // 4. Rebuild raw string ├óΓé¼ΓÇ¥ METHOD|PATH|TIMESTAMP|BODY
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

        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
        // Helpers
        // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
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
            11 => "Sanjay Gupta",
            12 => "Anita Jain",
            13 => "Rajesh Verma",
            14 => "Priya Singh",
            15 => "Amit Patel",
            16 => "Kiran Das",
            17 => "Vikram Rao",
            18 => "Neha Agarwal",
            19 => "Ravi Kumar",
            20 => "Sneha Bose",
            _  => $"Applicant {i}"
        };

        private record OfficeRef(
            string Code, int LevelId, string Name, string DistrictName);

        private record ServiceRef(
            int Code, string Name, int StimulateDays);
    }

    // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
    // Response wrapper
    // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
    public class ApiResponse<T>
    {
    [JsonPropertyName("success")]    public bool     Success    { get; set; }
    [JsonPropertyName("message")]    public string   Message    { get; set; } = string.Empty;
    [JsonPropertyName("payload_id")] public string   PayloadId  { get; set; } = string.Empty;
    [JsonPropertyName("totalCount")]   public int      TotalCount { get; set; }
    [JsonPropertyName("data")]       public List<T>  Data       { get; set; } = [];

    public static ApiResponse<T> Ok(List<T> data, int totalCount, string msg, string payloadId) => new()
        { Success = true, Message = msg, TotalCount = totalCount, Data = data, PayloadId = payloadId };

    public static ApiResponse<T> Empty(string msg) => new()
        { Success = true, Message = msg, TotalCount = 0, Data = [], PayloadId = string.Empty };
}

    // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼
    // DTOs
    // ├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼├óΓÇ¥Γé¼

    public class OfficeDto
    {
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
        [JsonPropertyName("jurisdiction_mode")]  public string? jurisdiction_mode { get; set; }
        [JsonPropertyName("jurisdiction_scope")]  public string? jurisdiction_scope { get; set; }
        [JsonPropertyName("jurisdiction")]        public JurisdictionDto? jurisdiction { get; set; }

    }

    public class ServiceDto
    {
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
        [JsonPropertyName("jurisdiction_mode")]  public string? jurisdiction_mode { get; set; }
        [JsonPropertyName("jurisdiction_scope")]  public string? jurisdiction_scope { get; set; }
        [JsonPropertyName("jurisdiction")]        public JurisdictionDto? jurisdiction { get; set; }
        [JsonPropertyName("beyond_department_days")]   [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int?  beyond_department_days  { get; set; }
        [JsonPropertyName("has_external_dependency")] public bool   has_external_dependency { get; set; } = false;
    }

    public class UserDto
    {
        [JsonPropertyName("official_email")]  public string  official_email  { get; set; } = default!;
        [JsonPropertyName("full_name")]       public string  full_name       { get; set; } = default!;
        [JsonPropertyName("mobile_no")]       public string? mobile_no       { get; set; }
        [JsonPropertyName("designation")]     public string  designation     { get; set; } = default!;
        [JsonPropertyName("role_key")]        public string  role_key        { get; set; } = default!;
        [JsonPropertyName("office_name")]     public string  office_name     { get; set; } = default!;
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
        [JsonPropertyName("service_name")]       public string  service_name       { get; set; } = default!;
        [JsonPropertyName("office_name")]        public string  office_name        { get; set; } = default!;
        [JsonPropertyName("official_email")]     public string? official_email     { get; set; }
        [JsonPropertyName("department_code")]    public string  department_code    { get; set; } = default!;
        [JsonPropertyName("present_status")]     public string  present_status     { get; set; } = default!;
        [JsonPropertyName("applied_date")]       public string? applied_date       { get; set; }
        [JsonPropertyName("last_updated_date")]  public string? last_updated_date  { get; set; }
        [JsonPropertyName("number_of_days_beyond_department_scope")] public int? NumberOfDaysBeyondDepartmentScope { get; set; }
    }

    /// <summary>
    /// ABC monthly MIS aggregate record.
    /// One record per service per period.
    /// AbcSyncService maps this to FormSubmissionRecordDto and calls
    /// IApplicationRecordService.AddBatchAsync ├óΓé¼ΓÇ¥ same as Form A portal.
    /// </summary>
    /// <summary>
    /// ABC monthly MIS aggregate record sent by the upstream ABC API.
    /// Uses integer IDs (service_id, office_id) matching rtps_wb.service and rtps_wb.office.
    /// AbcSyncService maps this directly to FormSubmissionRecordDto via IApplicationRecordService.AddBatchAsync.
    /// </summary>
    public class AbcSubmissionMisDto
    {
        [JsonPropertyName("external_record_id")]    public string  external_record_id    { get; set; } = string.Empty;
        [JsonPropertyName("dept_code")]             public string  dept_code             { get; set; } = string.Empty;
        [JsonPropertyName("department_code")]       public string  department_code       { get; set; } = string.Empty;
        [JsonPropertyName("dept_name")]             public string? dept_name             { get; set; }
        [JsonPropertyName("department_name")]       public string? department_name       { get; set; }
        [JsonPropertyName("office_name")]           public string? office_name           { get; set; }
        [JsonPropertyName("service_name")]          public string  service_name          { get; set; } = string.Empty;
        [JsonPropertyName("form_type")]             public string  form_type             { get; set; } = "A";
        [JsonPropertyName("officer_email")]         public string? officer_email         { get; set; }
        [JsonPropertyName("period_month")]          public int     period_month          { get; set; }
        [JsonPropertyName("period_year")]           public int     period_year           { get; set; }
        [JsonPropertyName("applications_received")] public int     applications_received { get; set; }
        [JsonPropertyName("form1_issued")]          public int     form1_issued          { get; set; }
        [JsonPropertyName("disposed_within_time")]  public int     disposed_within_time  { get; set; }
        [JsonPropertyName("disposed_after_time")]   public int     disposed_after_time   { get; set; }
        [JsonPropertyName("pending_applications")]  public int     pending_applications  { get; set; }
        [JsonPropertyName("appeals_received")]      public int     appeals_received      { get; set; }
        [JsonPropertyName("pending_appeals")]       public int     pending_appeals       { get; set; }
        [JsonPropertyName("external_updated_at")]   public DateTimeOffset external_updated_at { get; set; }
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
        [JsonPropertyName("reference_id")]   public string ReferenceId   { get; set; } = string.Empty;
        [JsonPropertyName("error_code")]     public string ErrorCode     { get; set; } = string.Empty;
        [JsonPropertyName("error_message")]  public string ErrorMessage  { get; set; } = string.Empty;
    }

    /// <summary>
    /// ABC monthly MIS aggregate record ├óΓé¼ΓÇ¥ one per service per period.
    /// AbcSyncService maps this to FormSubmissionRecordDto and calls
}


