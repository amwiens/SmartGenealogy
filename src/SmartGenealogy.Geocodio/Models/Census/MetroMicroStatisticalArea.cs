using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.Census;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio. Used when dealing with Census info.
/// </summary>
public class MetroMicroStatisticalArea
{
    [JsonConstructor]
    public MetroMicroStatisticalArea(string name, string area_code, string type)
    {
        Name = name;
        AreaCode = area_code;
        Type = type;
    }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("area_code")]
    public string AreaCode { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}