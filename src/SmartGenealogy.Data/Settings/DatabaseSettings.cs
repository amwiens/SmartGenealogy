namespace SmartGenealogy.Data.Settings;

/// <summary>
/// Database settings.
/// </summary>
public class DatabaseSettings
{
    /// <summary>
    /// Gets or sets the path to the database.
    /// </summary>
    public string? DatabasePath { get; set; }

    /// <summary>
    /// Gets or sets the filename of the database.
    /// </summary>
    public string? DatabaseFilename { get; set; }

    /// <summary>
    /// Gets the connection string to the database.
    /// </summary>
    public string? ConnectionString =>
        $"Data Source={Path.Combine(DatabasePath!, DatabaseFilename!)}";
}