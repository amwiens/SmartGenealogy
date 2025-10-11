namespace SmartGenealogy.Data.Models;

/// <summary>
/// Media link
/// </summary>
public class MediaLink
{
    /// <summary>
    /// Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Multimedia Id
    /// </summary>
    public int MultimediaId { get; set; }

    /// <summary>
    /// Owner type
    /// </summary>
    public OwnerType OwnerType { get; set; }

    /// <summary>
    /// Owner Id
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Is primary
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Comments
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Date added to the database
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// Date changed
    /// </summary>
    public DateTime DateChanged { get; set; }
}