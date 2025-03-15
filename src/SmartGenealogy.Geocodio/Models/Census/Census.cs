using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.Census;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio. Contains items related to the
/// US Census.
/// </summary>
public class Census
{
    [JsonPropertyName("census_year")]
    public int CensusYear { get; set; }

    [JsonPropertyName("state_fips")]
    public string? StateFIPS { get; set; }

    [JsonPropertyName("country_fips")]
    public string? CountryFIPS { get; set; }

    [JsonPropertyName("tract_code")]
    public string? TractCode { get; set; }

    [JsonPropertyName("block_code")]
    public string? BlockCode { get; set; }

    [JsonPropertyName("block_group")]
    public string? BlockGroup { get; set; }

    [JsonPropertyName("full_fips")]
    public string? Full_FIPS { get; set; }

    [JsonPropertyName("place")]
    public Place Place { get; set; }

    [JsonPropertyName("metro_micro_statistical_area")]
    public MetroMicroStatisticalArea MetroMicroStatisticalArea { get; set; }

    [JsonPropertyName("combined_statistical_area")]
    public CombinedStatisticalArea CombinedStatisticalArea { get; set; }

    [JsonPropertyName("metropolitan_division")]
    public CombinedStatisticalArea MetropolitanDivision { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }
}