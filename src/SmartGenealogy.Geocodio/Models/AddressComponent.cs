using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class AddressComponent
{
    [JsonConstructor]
    public AddressComponent(
        string number,
        string predirectional,
        string street,
        string suffix,
        string formattedstreet,
        string city,
        string county,
        string state,
        string zip,
        string country)
    {
        Number = number;
        Predirectional = predirectional;
        Street = street;
        Suffix = suffix;
        FormattedStreet = formattedstreet;
        City = city;
        County = county;
        State = state;
        Zip = zip;
        Country = country;
    }

    [JsonPropertyName("number")]
    public string Number { get; set; }
    [JsonPropertyName("predirectional")]
    public string Predirectional { get; set; }
    [JsonPropertyName("street")]
    public string Street { get; set; }
    [JsonPropertyName("suffix")]
    public string Suffix { get; set; }
    [JsonPropertyName("formatted_street")]
    public string FormattedStreet { get; set; }
    [JsonPropertyName("city")]
    public string City { get; set; }
    [JsonPropertyName("county")]
    public string County { get; set; }
    [JsonPropertyName("state")]
    public string State { get; set; }
    [JsonPropertyName("zip")]
    public string Zip { get; set; }
    [JsonPropertyName("country")]
    public string Country { get; set; }
}