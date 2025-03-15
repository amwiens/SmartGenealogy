using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.StateLegislature;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class StateLegislature
{
    private StateLegislature() { }

    [JsonConstructor]
    public StateLegislature(StateLegislator house, StateLegislator senate)
    {
        House = house;
        Senate = senate;
    }

    [JsonPropertyName("house")]
    public StateLegislator? House { get; set; }
    [JsonPropertyName("senate")]
    public StateLegislator? Senate { get; set; }
}