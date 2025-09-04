namespace SmartGenealogy.Data.Data;

public class DatabaseSettings
{
    public string? DatabaseName { get; set; }
    public string? DatabasePath { get; set; }
    public string? ConnectionString => !string.IsNullOrEmpty(DatabasePath) ? $"DataSource={DatabasePath};" : null;
}