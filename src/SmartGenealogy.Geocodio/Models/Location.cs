using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class Location
{
    [JsonConstructor]
    public Location(decimal latitude, decimal longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    [JsonPropertyName("lat")]
    public decimal Latitude { get; set; }
    [JsonPropertyName("lng")]
    public decimal Longitude { get; set; }
}