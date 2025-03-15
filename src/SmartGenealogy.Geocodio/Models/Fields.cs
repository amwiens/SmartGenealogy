using System.Text.Json.Serialization;

using SmartGenealogy.Geocodio.Models.Census;
using SmartGenealogy.Geocodio.Models.Census.CanadianCensus;
using SmartGenealogy.Geocodio.Models.FederalLegislature;
using SmartGenealogy.Geocodio.Models.SchoolDistrict;

namespace SmartGenealogy.Geocodio.Models;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class Fields
{
    /// <summary>
    /// US Federal legislature information.
    /// </summary>
    [JsonPropertyName("congressional_districts")]
    public CongressionalDistrict[]? CongressionalDistricts { get; set; }

    /// <summary>
    /// US State legislature information.
    /// </summary>
    [JsonPropertyName("state_legislative_districts")]
    public StateLegislature.StateLegislature? StateLegislature { get; set; }

    /// <summary>
    /// US school district information.
    /// </summary>
    [JsonPropertyName("school_districts")]
    public SchoolDistricts? SchoolDistricts { get; set; }

    /// <summary>
    /// Timezone information. The standardized name follows the tzdb format. E.g. America/New_York.
    /// </summary>
    [JsonPropertyName("timezone")]
    public TimeZone? TimeZone { get; set; }

    /// <summary>
    /// US Census information.
    /// </summary>
    [JsonPropertyName("census")]
    public Dictionary<string, Census.Census>? Census { get; set; }

    /// <summary>
    /// American Community Survey, a more frequent version of the decennial census. See: https://en.wikipedia.org/wiki/American_Community_Survey
    /// </summary>
    [JsonPropertyName("acs")]
    public ACS? ACS_Results { get; set; }

    /// <summary>
    /// Canadian census field for Canadian electoral district. See: https://en.wikipedia.org/wiki/List_of_Canadian_federal_electoral_districts
    /// </summary>
    public Riding? Riding { get; set; }

    /// <summary>
    /// Census geographic units of Cana. See: https://en.wikipedia.org/wiki/Census_geographic_units_of_Canada
    /// </summary>
    public StatCan? StatCan { get; set; }
}