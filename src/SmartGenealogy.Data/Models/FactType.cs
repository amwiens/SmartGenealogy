namespace SmartGenealogy.Data.Models;

public class FactType
{
    public int Id { get; set; }
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