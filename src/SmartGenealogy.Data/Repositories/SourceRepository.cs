using SmartGenealogy.Data.Models;

namespace SmartGenealogy.Data.Repositories;

/// <summary>
/// Repository class for managing Source entities in the database.
/// </summary>
/// <remarks>
/// INitializes a new instance of the <see cref="SourceRepository"/> class.
/// </remarks>
/// <param name="databaseSettings">Database settings.</param>
/// <param name="logger">Logger.</param>
public class SourceRepository(DatabaseSettings databaseSettings, ILogger<SourceRepository> logger)
{
    private bool _hasBeenInitialized = false;

    /// <summary>
    /// Initializes the database connection and creates the Source table if it does not exist.
    /// </summary>
    private async Task Init()
    {
        if (_hasBeenInitialized)
            return;

        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        try
        {
            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText =
                @"CREATE TABLE IF NOT EXISTS Source (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    RefNumber TEXT,
                    ActualText TEXT,
                    Comments TEXT,
                    IsPrivate INTEGER,
                    TemplateId INTEGER,
                    Fields TEXT,
                    DateAdded TEXT,
                    DateChanged TEXT
                );";
            await createTableCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating Source table.");
            throw;
        }

        // Initialization logic for the Source table would go here.
        _hasBeenInitialized = true;
    }

    /// <summary>
    /// Retrieves a list of all sources from the database.
    /// </summary>
    /// <returns>A list of <see cref="Source"/> objects.</returns>
    public async Task<List<Source>> ListAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Source";
        var sources = new List<Source>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            sources.Add(new Source
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                RefNumber = reader.IsDBNull(reader.GetOrdinal("RefNumber")) ? null : reader.GetString(reader.GetOrdinal("RefNumber")),
                ActualText = reader.IsDBNull(reader.GetOrdinal("ActualText")) ? null : reader.GetString(reader.GetOrdinal("ActualText")),
                Comments = reader.IsDBNull(reader.GetOrdinal("Comments")) ? null : reader.GetString(reader.GetOrdinal("Comments")),
                IsPrivate = reader.GetBoolean(reader.GetOrdinal("IsPrivate")),
                TemplateId = reader.GetInt32(reader.GetOrdinal("TemplateId")),
                Fields = reader.IsDBNull(reader.GetOrdinal("Fields")) ? null : reader.GetString(reader.GetOrdinal("Fields")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        return sources;
    }

    /// <summary>
    /// Retrieves a specific source by its ID.
    /// </summary>
    /// <param name="id">The ID of the source.</param>
    /// <returns>A <see cref="Source"/> object if found; otherwise, null.</returns>
    public async Task<Source?> GetAsync(int id)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Source WHERE Id = @id";
        selectCmd.Parameters.AddWithValue("@id", id);

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var source = new Source
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                RefNumber = reader.IsDBNull(reader.GetOrdinal("RefNumber")) ? null : reader.GetString(reader.GetOrdinal("RefNumber")),
                ActualText = reader.IsDBNull(reader.GetOrdinal("ActualText")) ? null : reader.GetString(reader.GetOrdinal("ActualText")),
                Comments = reader.IsDBNull(reader.GetOrdinal("Comments")) ? null : reader.GetString(reader.GetOrdinal("Comments")),
                IsPrivate = reader.GetBoolean(reader.GetOrdinal("IsPrivate")),
                TemplateId = reader.GetInt32(reader.GetOrdinal("TemplateId")),
                Fields = reader.IsDBNull(reader.GetOrdinal("Fields")) ? null : reader.GetString(reader.GetOrdinal("Fields")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            };

            return source;
        }

        return null;
    }

    /// <summary>
    /// Saves a source to the database. If the role id is 0, a new source is created; otherwise, the existing source is updated.
    /// </summary>
    /// <param name="item">The source to save.</param>
    /// <returns>The Id of the saved source.</returns>
    public async Task<int> SaveItemAsync(Source item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var saveCmd = connection.CreateCommand();
        if  (item.Id == 0)
        {
            saveCmd.CommandText = @"
                INSERT INTO Source (Name, RefNumber, ActualText, Comments, IsPrivate, TemplateId, Fields, DateAdded, DateChanged)
                VALUES (@Name, @RefNumber, @ActualText, @Comments, @IsPrivate, @TemplateId, @Fields, @DateAdded, @DateChanged);
                SELECT last_insert_rowid();";
            saveCmd.Parameters.AddWithValue("@DateAdded", DateTime.UtcNow);
        }
        else
        {
            saveCmd.CommandText = @"
                UPDATE Source
                SET Name = @Name,
                    RefNumber = @RefNumber,
                    ActualText = @ActualText,
                    Comments = @Comments,
                    IsPrivate = @IsPrivate,
                    TemplateId = @TemplateId,
                    Fields = @Fields,
                    DateChanged = @DateChanged
                WHERE Id = @id";
            saveCmd.Parameters.AddWithValue("@id", item.Id);
        }

        saveCmd.Parameters.AddWithValue("@Name", item.Name ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@RefNumber", item.RefNumber ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@ActualText", item.ActualText ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@Comments", item.Comments ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@IsPrivate", item.IsPrivate ? 1 : 0);
        saveCmd.Parameters.AddWithValue("@TemplateId", item.TemplateId);
        saveCmd.Parameters.AddWithValue("@Fields", item.Fields ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@DateChanged", DateTime.UtcNow);

        var result = await saveCmd.ExecuteScalarAsync();
        if (item.Id == 0)
        {
            item.Id = Convert.ToInt32(result);
        }

        return item.Id;
    }

    /// <summary>
    /// Deletes a source from the database.
    /// </summary>
    /// <param name="item">The source to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(Source item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM Source WHERE Id = @id";
        deleteCmd.Parameters.AddWithValue("@id", item.Id);

        return await deleteCmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Creates the Source table in the database.
    /// </summary>
    public async Task CreateTableAsync()
    {
        await Init();
    }

    /// <summary>
    /// Drops the Source table from the database.
    /// </summary>
    public async Task DropTableAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var dropTableCommand = connection.CreateCommand();
        dropTableCommand.CommandText = "DROP TABLE IF EXISTS Source";
        await dropTableCommand.ExecuteNonQueryAsync();

        _hasBeenInitialized = false;
    }
}