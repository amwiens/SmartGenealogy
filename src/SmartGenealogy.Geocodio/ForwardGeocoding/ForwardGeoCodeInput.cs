using System.Text.Json.Serialization;

using SmartGenealogy.Geocodio.Models;

namespace SmartGenealogy.Geocodio.ForwardGeocoding;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class ForwardGeoCodeInput
{
    private ForwardGeoCodeInput()
    { }

    [JsonConstructor]
    public ForwardGeoCodeInput(AddressComponent input, string formattedAddress)
    {
        Input = input;
        FormattedAddress = formattedAddress;
    }

    public AddressComponent? Input { get; set; }

    [JsonPropertyName("formatted_address")]
    public string? FormattedAddress { get; set; }
}