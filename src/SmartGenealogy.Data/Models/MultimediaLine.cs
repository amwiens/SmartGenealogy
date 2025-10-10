namespace SmartGenealogy.Data.Models;

public class MultimediaLine
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Multimedia Id
    /// </summary>
    public int MultimediaId { get; set; }

    /// <summary>
    /// Line number
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// Text
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Date added to the database
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// Date changed
    /// </summary>
    public DateTime DateChanged { get; set; }
}