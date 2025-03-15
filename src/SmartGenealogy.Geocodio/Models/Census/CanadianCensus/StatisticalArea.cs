using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.Census.CanadianCensus;

/// <summary>
/// Contains details related to StatCan or Riding fields.
/// </summary>
public class StatisticalArea
{
    public string? Code { get; set; }

    [JsonPropertyName("code_description")]
    public string? CodeDescription { get; set; }

    public string? Type { get; set; }

    [JsonPropertyName("type_description")]
    public string? TypeDescription { get; set; }
}