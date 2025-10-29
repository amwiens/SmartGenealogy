namespace SmartGenealogy.Data.Models;

/// <summary>
/// Web link link
/// </summary>
public class WebLinkLink
{
    /// <summary>
    /// Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Web link Id
    /// </summary>
    public int WebLinkId { get; set; }

    /// <summary>
    /// Owner type
    /// </summary>
    public OwnerType OwnerType { get; set; }

    /// <summary>
    /// Owner Id
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Date added to the database
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// Date changed
    /// </summary>
    public DateTime DateChanged { get; set; }

    /// <summary>
    /// Web link
    /// </summary>
    public WebLink? WebLink { get; set; }
}