using SQLite;

namespace SmartGenealogy.Models;

public class Place
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Notes { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.Now;
    public DateTime DateChanged { get; set; }
}