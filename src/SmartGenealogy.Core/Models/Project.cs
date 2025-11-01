namespace SmartGenealogy.Core.Models;

/// <summary>
/// Project model.
/// </summary>
public class Project
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
    /// Description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Priority
    /// </summary>
    public Priority? Priority { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public ProjectStatus? Status { get; set; }

    /// <summary>
    /// Category
    /// </summary>
    public ProjectCategory? Category { get; set; }


    public string? CategoryDescription => EnumHelper.GetEnumDescription(Category!) ?? string.Empty;

    /// <summary>
    /// Start date
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Date added to the database
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// Local date added to the database
    /// </summary>
    public DateTime DateAddedLocal => DateAdded.ToLocalTime();

    /// <summary>
    /// Date added relative
    /// </summary>
    public string DateAddedRelative => DateAddedLocal.Humanize();

    /// <summary>
    /// Date changed
    /// </summary>
    public DateTime DateChanged { get; set; }

    /// <summary>
    /// Local date changed
    /// </summary>
    public DateTime DateChangedLocal => DateChanged.ToLocalTime();
}