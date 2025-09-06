namespace SmartGenealogy.Data.Models;

public class FactType
{
    [PrimaryKey, AutoIncrement]
    public long Id { get; set; }
    public OwnerType OwnerType { get; set; }
    public string? Name { get; set; }
    public string? Abbreviation { get; set; }
    public string? GedcomTag { get; set; }
    public bool UseValue { get; set; }
    public bool UseDate { get; set; }
    public bool UsePlace { get; set; }
    public string? Sentence { get; set; }
    public bool IsBuiltIn { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime DateChanged { get; set; }
}

public class FactTypesJson
{
    public List<FactType> FactTypes { get; set; } = [];
}