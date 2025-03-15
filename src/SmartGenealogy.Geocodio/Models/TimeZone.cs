using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class TimeZone
{
    private TimeZone()
    { }

    [JsonConstructor]
    public TimeZone(string name, decimal utc_offset, bool observes_dst, string abbreviation, string source)
    {
        Name = name;
        UTC_Offset = utc_offset;
        Observes_DST = observes_dst;
        Abbreviation = abbreviation;
        Source = source;
    }

    public string? Name { get; set; }

    [JsonPropertyName("utc_offset")]
    public decimal UTC_Offset { get; set; }

    [JsonPropertyName("observes_dst")]
    public bool Observes_DST { get; set; }

    public string? Abbreviation { get; set; }
    public string? Source { get; set; }
}