using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.Census;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio. Used when dealing with Census info.
/// </summary>
public class ACS
{
    [JsonPropertyName("meta")]
    public ACS_Meta Meta { get; set; }

    [JsonPropertyName("demographics")]
    public Dictionary<string, Dictionary<string, Dictionary<string, string>>>? Demographics { get; set; }

    [JsonPropertyName("economics")]
    public Dictionary<string, Dictionary<string, Dictionary<string, string>>>? Economics { get; set; }

    [JsonPropertyName("families")]
    public Dictionary<string, Dictionary<string, Dictionary<string, string>>>? Families { get; set; }

    [JsonPropertyName("housing")]
    public Dictionary<string, Dictionary<string, Dictionary<string, string>>>? Housing { get; set; }

    [JsonPropertyName("social")]
    public Dictionary<string, Dictionary<string, Dictionary<string, string>>>? Social { get; set; }
}