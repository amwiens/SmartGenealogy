namespace SmartGenealogy.Settings;

public class DatabaseSettings
{
    public string? DatabaseName { get; set; }
    public string? DatabasePath { get; set; }
    public string? connectionString => !string.IsNullOrEmpty(DatabasePath) ? $"DataSource={DatabasePath};" : null;
}