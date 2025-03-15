using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.ForwardGeocoding;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class BatchForwardGeoCodeResult
{
    private BatchForwardGeoCodeResult() { }

    [JsonConstructor]
    public BatchForwardGeoCodeResult(BatchForwardGeoCodeRecord[] results)
    {
        Results = results;
    }

    public BatchForwardGeoCodeRecord[] Results { get; set; }
}