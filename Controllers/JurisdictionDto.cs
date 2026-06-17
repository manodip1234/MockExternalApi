using System.Text.Json.Serialization;

namespace MockExternalApi.Controllers;

// Flat geo entry used inside a jurisdiction group's "jurisdiction" array
public sealed class JurisdictionDto
{
    [JsonPropertyName("notes")]              [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? notes              { get; set; }
    [JsonPropertyName("state_id")]           [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int?    state_id           { get; set; }
    [JsonPropertyName("district_id")]        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int?    district_id        { get; set; }
    [JsonPropertyName("block_id")]           [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int?    block_id           { get; set; }
    [JsonPropertyName("municipality_id")]    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int?    municipality_id    { get; set; }
    [JsonPropertyName("ward_id")]            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int?    ward_id            { get; set; }
    [JsonPropertyName("state_name")]         [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? state_name         { get; set; }
    [JsonPropertyName("district_name")]      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? district_name      { get; set; }
    [JsonPropertyName("block_name")]         [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? block_name         { get; set; }
    [JsonPropertyName("municipality_name")]  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? municipality_name  { get; set; }
    [JsonPropertyName("ward_name")]          [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? ward_name          { get; set; }
}

// One group inside jurisdiction_groups
public sealed class JurisdictionGroupDto
{
    [JsonPropertyName("group_name")]   public string               group_name   { get; set; } = default!;
    [JsonPropertyName("jurisdiction")] public List<JurisdictionDto> jurisdiction { get; set; } = [];
}
