namespace SmartGenealogy.Data.Models;

/// <summary>
/// Multimedia
/// </summary>
public class Multimedia
{
    /// <summary>
    /// Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Media type
    /// </summary>
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Media path
    /// </summary>
    public string? MediaPath { get; set; }

    /// <summary>
    /// Media file
    /// </summary>
    public string? MediaFile { get; set; }

    /// <summary>
    /// Url
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Thumbnail
    /// </summary>
    public byte[]? Thumbnail { get; set; }

    /// <summary>
    /// Caption
    /// </summary>
    public string? Caption { get; set; }

    /// <summary>
    /// Ref number
    /// </summary>
    public string? RefNumber { get; set; }

    /// <summary>
    /// Date
    /// </summary>
    public string? Date { get; set; }

    /// <summary>
    /// Sort date
    /// </summary>
    public int SortDate { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Date added to the database
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// Date changed
    /// </summary>
    public DateTime DateChanged { get; set; }

}