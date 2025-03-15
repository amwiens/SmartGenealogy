using System.Text.Json.Serialization;

using SmartGenealogy.Geocodio.Models;

namespace SmartGenealogy.Geocodio.ReverseGeocoding;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class ReverseGeoCodeResult
{
    [JsonConstructor]
    public ReverseGeoCodeResult(GeoCodeInfo[] results)
    {
        Results = results;
    }

    [JsonPropertyName("results")]
    public GeoCodeInfo[] Results { get; set; }

    public string[]? _warnings { get; set; }
}