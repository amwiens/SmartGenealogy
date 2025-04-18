namespace SmartGenealogy.Data.Models;

public class Place
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? Name { get; set; }
}