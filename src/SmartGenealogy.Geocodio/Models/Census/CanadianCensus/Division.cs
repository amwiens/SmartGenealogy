using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.Census.CanadianCensus;

/// <summary>
/// JSON backer class used when querying Canadian census fields.
/// </summary>
public class Division
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }

    [JsonPropertyName("type_description")]
    public string? TypeDescription { get; set; }
}