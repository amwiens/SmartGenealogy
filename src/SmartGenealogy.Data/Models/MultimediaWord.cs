namespace SmartGenealogy.Data.Models;

public class MultimediaWord
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
    /// Confidence
    /// </summary>
    public float Confidence { get; set; }

    /// <summary>
    /// Height
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Text
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Width
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// X
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Y
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Date added to the database
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// Date changed
    /// </summary>
    public DateTime DateChanged { get; set; }
}