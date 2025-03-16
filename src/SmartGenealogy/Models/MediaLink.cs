using SmartGenealogy.Enums;

using SQLite;

namespace SmartGenealogy.Models;

public class MediaLink
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int MediaId { get; set; }
    public OwnerType OwnerType { get; set; }
    public int OwnerId { get; set; }
    public DateTime? DateAdded { get; set; } = DateTime.Now;
    public DateTime? DateChanged { get; set; }
}