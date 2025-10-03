namespace SmartGenealogy.Data.Settings;

public class DatabaseSettings
{
    public string? DatabasePath { get; set; }

    public string? DatabaseFilename { get; set; }

    public string? ConnectionString =>
        $"Data Source={Path.Combine(DatabasePath!, DatabaseFilename!)}";
}