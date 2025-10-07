namespace SmartGenealogy.Data.Repositories;

/// <summary>
/// Database tools
/// </summary>
/// <param name="databaseSettings">Database settings</param>
/// <param name="logger">Logger</param>
public class DatabaseTools(DatabaseSettings databaseSettings, ILogger<DatabaseTools> logger)
{
    /// <summary>
    /// Compact database.
    /// </summary>
    public async Task CompactDatabase()
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        try
        {
            var compactCommand = connection.CreateCommand();
            compactCommand.CommandText = "VACUUM;";

            await compactCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error compacting database.");
            throw;
        }
    }

    /// <summary>
    /// Reindex database.
    /// </summary>
    public async Task ReindexDatabase()
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        try
        {
            var compactCommand = connection.CreateCommand();
            compactCommand.CommandText = "REINDEX;";

            await compactCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error rebuiding indexes in database.");
            throw;
        }
    }

    /// <summary>
    /// Database integrity check.
    /// </summary>
    public async Task DatabaseIntegrityCheck()
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        try
        {
            var compactCommand = connection.CreateCommand();
            compactCommand.CommandText = "PRAGMA integrity_check;";

            await compactCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error running integrity check in database.");
            throw;
        }
    }
}