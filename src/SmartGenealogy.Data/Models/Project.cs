namespace SmartGenealogy.Data.Models;

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
    /// Date changed
    /// </summary>
    public DateTime DateChanged { get; set; }
}