namespace SmartGenealogy.Data.Models;

/// <summary>
/// Role
/// </summary>
public class Role
{
    /// <summary>
    /// Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the role.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Event type
    /// </summary>
    public int EventType { get; set; }

    /// <summary>
    /// Role type
    /// </summary>
    public int RoleType { get; set; }

    /// <summary>
    /// Sentence
    /// </summary>
    public string? Sentence { get; set; }

    /// <summary>
    /// Date added to the database
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// Date changed
    /// </summary>
    public DateTime DateChanged { get; set; }
}