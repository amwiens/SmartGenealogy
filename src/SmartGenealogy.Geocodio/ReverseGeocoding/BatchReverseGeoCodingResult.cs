using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.ReverseGeocoding;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class BatchReverseGeoCodingResult
{
    [JsonConstructor]
    public BatchReverseGeoCodingResult(BatchReverseGeoCodeResponse[] results)
    {
        Results = results;
    }

    [JsonPropertyName("results")]
    public BatchReverseGeoCodeResponse[] Results { get; set; }
}