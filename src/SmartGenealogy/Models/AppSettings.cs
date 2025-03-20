namespace SmartGenealogy.Models;

public class AppSettings
{
    public bool DarkMode { get; set; }
    public string? TesseractLanguageFileLocation { get; set; }
    public string? PlacesBaseDirectory { get; set; }
    public string? GeocodioApiKey { get; set; }
}