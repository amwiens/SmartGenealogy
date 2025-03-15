using System.Text.Json.Serialization;

using SmartGenealogy.Geocodio.Models;

namespace SmartGenealogy.Geocodio.ForwardGeocoding;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class ForwardGeoCodeResult
{
    private ForwardGeoCodeResult()
    { }

    [JsonConstructor]
    public ForwardGeoCodeResult(ForwardGeoCodeInput input, GeoCodeInfo[] results)
    {
        Input = input;
        Results = results;
    }

    [JsonPropertyName("input")]
    public ForwardGeoCodeInput? Input { get; set; }

    [JsonPropertyName("results")]
    public GeoCodeInfo[]? Results { get; set; }

    public string[]? _warnings { get; set; }
}