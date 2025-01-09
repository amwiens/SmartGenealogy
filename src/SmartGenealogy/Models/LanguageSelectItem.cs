namespace SmartGenealogy.Models;

public class LanguageSelectItem
{
    public required string Code { get; set; }
    public required string Flag { get; set; }
    public required string Name { get; set; }
    public bool IsRTL { get; set; }
}