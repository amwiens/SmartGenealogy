namespace SmartGenealogy.Data.Models;

public class Person
{
    [PrimaryKey, AutoIncrement]
    public long Id { get; set; }
    public long Sex { get; set; }
    public long ParentID { get; set; }

}