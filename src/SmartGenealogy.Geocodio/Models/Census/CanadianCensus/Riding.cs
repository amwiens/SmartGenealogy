using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.Census.CanadianCensus;

/// <summary>
/// Canadian census field for Canadian electoral district. See: https://en.wikipedia.org/siki/List_of_Canadian_federal_electoral_districts
/// </summary>
public class Riding
{
    public string? Code { get; set; }
    [JsonPropertyName("name_french")]
    public string? NameFrench { get; set; }
    [JsonPropertyName("name_english")]
    public string? NameEnglish { get; set; }
}