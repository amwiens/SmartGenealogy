namespace SmartGenealogy.Models;

public class GeocodedPlace
{
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? County { get; set; }
    public string? City { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}