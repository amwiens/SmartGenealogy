namespace SmartGenealogy.Geocodio.Models.Census;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio. Used when dealing with Census info.
/// </summary>
public class Place
{
    public string? Name { get; set; }
    public string? FIPS { get; set; }
}