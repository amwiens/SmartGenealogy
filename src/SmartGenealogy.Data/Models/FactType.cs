namespace SmartGenealogy.Data.Models;

/// <summary>
/// Fact type
/// </summary>
public class FactType
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Owner type
    /// </summary>
    public OwnerType OwnerType { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Abbreviation
    /// </summary>
    public string? Abbreviation { get; set; }

    /// <summary>
    /// Gedcom tag
    /// </summary>
    public string? GedcomTag { get; set; }

    /// <summary>
    /// Whether the fact type uses the value field.
    /// </summary>
    public bool UseValue { get; set; }

    /// <summary>
    /// Whether the fact type uses the date field.
    /// </summary>
    public bool UseDate { get; set; }

    /// <summary>
    /// Whether the fact type uses the place fields.
    /// </summary>
    public bool UsePlace { get; set; }

    /// <summary>
    /// Sentence
    /// </summary>
    public string? Sentence { get; set; }

    /// <summary>
    /// Whether the fact type is built-in and cannot be deleted.
    /// </summary>
    public bool IsBuiltIn { get; set; }

    /// <summary>
    /// Date added to the database
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// Date changed
    /// </summary>
    public DateTime DateChanged { get; set; }
}