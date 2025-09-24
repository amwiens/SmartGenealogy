namespace SmartGenealogy.Data.Settings;

/// <summary>
/// Used to hold the database settings.
/// </summary>
public class DatabaseSettings
{
    /// <summary>
    /// Database name.
    /// </summary>
    public string? DatabaseName { get; set; }

    /// <summary>
    /// Database path.
    /// </summary>
    public string? DatabasePath { get; set; }

    /// <summary>
    /// Connection string.
    /// </summary>
    public string? ConnectionString => !string.IsNullOrEmpty(DatabasePath) ? $"DataSource={Path.Combine(DatabasePath, DatabaseName!)};" : null;
}