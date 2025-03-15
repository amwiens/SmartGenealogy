using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.Census;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio. Used when dealing with Census info.
/// </summary>
public class CombinedStatisticalArea
{
    [JsonConstructor]
    public CombinedStatisticalArea(string name, string area_code)
    {
        Name = name;
        AreaCode = area_code;
    }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("area_code")]
    public string? AreaCode { get; set; }
}