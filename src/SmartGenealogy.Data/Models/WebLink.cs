namespace SmartGenealogy.Data.Models;

/// <summary>
/// Web link
/// </summary>
public class WebLink
{
    /// <summary>
    /// Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Owner type
    /// </summary>
    public OwnerType OwnerType { get; set; }

    /// <summary>
    /// Owner Id
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Link type
    /// </summary>
    public int LinkType { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// URL
    /// </summary>
    public string? URL { get; set; }

    /// <summary>
    /// Note
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Date added to the database
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// Date changed
    /// </summary>
    public DateTime DateChanged { get; set; }
}