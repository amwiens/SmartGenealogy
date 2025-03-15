using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.Census.CanadianCensus;

/// <summary>
/// Census geographic units of Cana. See: https://en.wikipedia.org/wiki/Census_geographic_units_of_Canada
/// </summary>
public class StatCan
{
    public Division? Division { get; set; }
    [JsonPropertyName("consolidated_subdivision")]
    public Division? ConsolidatedSubdivision { get; set; }
    public Division? Subdivision { get; set; }
    [JsonPropertyName("economic_region")]
    public string? EconomicRegion { get; set; }
    [JsonPropertyName("statistical_area")]
    public StatisticalArea? StatisticalArea { get; set; }
    [JsonPropertyName("cma_ca")]
    public Division? CMA_CA { get; set; }
    public string? Tract { get; set; }
    [JsonPropertyName("census_year")]
    public int CensusYear { get; set; }
}