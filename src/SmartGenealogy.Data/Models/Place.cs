namespace SmartGenealogy.Data.Models;

/// <summary>
/// Place
/// </summary>
public class Place
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Place type
    /// </summary>
    public PlaceType PlaceType { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Abbreviation
    /// </summary>
    public string? Abbreviation { get; set; }

    /// <summary>
    /// Normalized
    /// </summary>
    public string? Normalized { get; set; }

    /// <summary>
    /// Latitude
    /// </summary>
    public decimal Latitude { get; set; }

    /// <summary>
    /// Longitude
    /// </summary>
    public decimal Longitude { get; set; }

    /// <summary>
    /// Master Id
    /// </summary>
    public int MasterId { get; set; }

    /// <summary>
    /// Note
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Reverse
    /// </summary>
    public string? Reverse { get; set; }

    /// <summary>
    /// Date added
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// Date changed
    /// </summary>
    public DateTime DateChanged { get; set; }

    /// <summary>
    /// Place details
    /// </summary>
    public List<Place>? PlaceDetails { get; set; }

    /// <summary>
    /// Media links
    /// </summary>
    public List<MediaLink>? MediaLinks { get; set; }
}