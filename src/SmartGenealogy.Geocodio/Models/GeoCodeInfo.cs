using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class GeoCodeInfo
{
    [JsonConstructor]
    public GeoCodeInfo(
        AddressComponent addressComponents,
        string formattedAddress,
        Location location,
        double accuracy,
        string accuracyType,
        string source,
        Fields fields)
    {
        AddressComponents = addressComponents;
        FormattedAddress = formattedAddress;
        Location = location;
        Accuracy = accuracy;
        AccuracyType = accuracyType;
        Source = source;
        Fields = fields;
    }

    [JsonPropertyName("address_components")]
    public AddressComponent AddressComponents { get; set; }

    [JsonPropertyName("formatted_address")]
    public string? FormattedAddress { get; set; }

    [JsonPropertyName("location")]
    public Location Location { get; set; }

    [JsonPropertyName("accuracy")]
    public double Accuracy { get; set; }

    [JsonPropertyName("accuracy_type")]
    public string? AccuracyType { get; set; }

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("fields")]
    public Fields Fields { get; set; }
}