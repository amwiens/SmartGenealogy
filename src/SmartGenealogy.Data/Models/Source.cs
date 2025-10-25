namespace SmartGenealogy.Data.Models;

/// <summary>
/// Source
/// </summary>
public class Source
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Reference number
    /// </summary>
    public string? RefNumber { get; set; }

    /// <summary>
    /// Actual text
    /// </summary>
    public string? ActualText { get; set; }

    /// <summary>
    /// Comments
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Is private
    /// </summary>
    public bool IsPrivate { get; set; }

    /// <summary>
    /// Template id
    /// </summary>
    public int TemplateId { get; set; }

    /// <summary>
    /// Fields
    /// </summary>
    public string? Fields { get; set; }

    /// <summary>
    /// Date added to the database
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// Date changed
    /// </summary>
    public DateTime DateChanged { get; set; }
}