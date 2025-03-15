using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.SchoolDistrict;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class SchoolDistricts
{
    private SchoolDistricts() { }

    [JsonConstructor]
    public SchoolDistricts(
        SchoolDistrict? unified = null,
        SchoolDistrict? elementary = null,
        SchoolDistrict? secondary = null)
    {
        Unified = unified;
        Elementary = elementary;
        Secondary = secondary;
    }

    [JsonPropertyName("unified")]
    public SchoolDistrict? Unified { get; set; }
    [JsonPropertyName("elementary")]
    public SchoolDistrict? Elementary { get; set; }
    [JsonPropertyName("secondary")]
    public SchoolDistrict? Secondary { get; set; }
}