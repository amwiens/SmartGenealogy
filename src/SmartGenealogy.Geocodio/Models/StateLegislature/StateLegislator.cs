using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.StateLegislature;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class StateLegislator
{
    private StateLegislator() { }

    [JsonConstructor]
    public StateLegislator(string name, string districtNumber)
    {
        Name = name;
        DistrictNumber = districtNumber;
    }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("district_number")]
    public string? DistrictNumber { get; set; }
}