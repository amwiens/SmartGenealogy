using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.FederalLegislature;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class CongressionalDistrict
{
    [JsonConstructor]
    public CongressionalDistrict(
        string name,
        int districtNumber,
        string congressNumber,
        string congressYears,
        int proportion,
        FederalLegislator[] currentLegislators)
    {
        Name = name;
        DistrictNumber = districtNumber;
        CongressNumber = congressNumber;
        CongressYears = congressYears;
        Proportion = proportion;
        CurrentLegislators = currentLegislators;
    }

    public string Name { get; set; }

    [JsonPropertyName("district_number")]
    public int DistrictNumber { get; set; }

    [JsonPropertyName("congress_number")]
    public string CongressNumber { get; set; }

    [JsonPropertyName("congress_years")]
    public string CongressYears { get; set; }

    public int Proportion { get; set; }

    [JsonPropertyName("current_legislators")]
    public FederalLegislator[] CurrentLegislators { get; set; }
}