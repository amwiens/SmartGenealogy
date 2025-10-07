namespace SmartGenealogy.Data.Repositories;

/// <summary>
/// Database tools
/// </summary>
/// <param name="databaseSettings">Database settings</param>
/// <param name="logger">Logger</param>
public class DatabaseTools(DatabaseSettings databaseSettings, ILogger<DatabaseTools> logger)
{

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
}