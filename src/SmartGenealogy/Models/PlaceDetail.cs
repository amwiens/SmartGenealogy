using SmartGenealogy.Enums;

using SQLite;

namespace SmartGenealogy.Models;

public class PlaceDetail
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int PlaceId { get; set; }
    public string? Name { get; set; }
    public PlaceDetailType Type { get; set; }
    public string? Address { get; set; }
    public string? Notes { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.Now;
    public DateTime DateChanged { get; set; }
}