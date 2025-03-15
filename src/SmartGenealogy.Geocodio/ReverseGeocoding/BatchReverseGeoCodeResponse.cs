using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.ReverseGeocoding;

/// <summary>
/// JSON backer class used to deserialize responses from Geocodio.
/// </summary>
public class BatchReverseGeoCodeResponse
{
    [JsonConstructor]
    public BatchReverseGeoCodeResponse(string query, ReverseGeoCodeResult response)
    {
        Query = query;
        Response = response;
    }

    [JsonPropertyName("query")]
    public string Query { get; set; }
    [JsonPropertyName("reponse")]
    public ReverseGeoCodeResult Response { get; set; }
}