using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.Census;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio. Used when dealing with Census info.
/// </summary>
public class ACS_Meta
{
    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("survey_years")]
    public string? SurveyYears { get; set; }

    [JsonPropertyName("survey_duration_years")]
    public string? SurveyDurationYears { get; set; }
}