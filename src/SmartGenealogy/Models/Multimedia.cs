using SQLite;

namespace SmartGenealogy.Models;

public class Multimedia
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int MediaType { get; set; }
    public string? MediaPath { get; set; }
    public string? MediaFile { get; set; }
    public string? Caption { get; set; }
    public string? Date { get; set; }
    public string? RefNumber { get; set; }
    public string? Text { get; set; }
    public DateTime? DateAdded { get; set; } = DateTime.Now;
    public DateTime? DateChanged { get; set; }
}