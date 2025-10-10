namespace SmartGenealogy.Data.Repositories;

/// <summary>
/// Repository class for managing Multimedia lines entities in the database.
/// </summary>
/// <param name="databaseSettings">Database settings</param>
/// <param name="logger">Logger</param>
public class MultimediaLineRepository(
    DatabaseSettings databaseSettings,
    ILogger<MultimediaLineRepository> logger)
{
    private bool _hasBeenInitialized = false;

    /// <summary>
    /// Initializes the database connection and creates the Multimedia lines table if it does not exist.
    /// </summary>
    /// <returns></returns>
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
                @"CREATE TABLE IF NOT EXISTS MultimediaLine(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    MultimediaId INTEGER NOT NULL,
                    LineNumber INTEGER NOT NULL,
                    Text TEXT NOT NULL,
                    DateAdded TEXT NOT NULL,
                    DateChanged TEXT NOT NULL
                );";
            await createTableCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating Multimedia lines table.");
            throw;
        }

        // Initialization logic for the Multimedia lines table would go here.
        _hasBeenInitialized = true;
    }

    /// <summary>
    /// Retrieves a list of all multimedia lines from the database.
    /// </summary>
    /// <returns>A list of <see cref="MultimediaLine"/> objects.</returns>
    public async Task<List<MultimediaLine>> ListAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM MultimediaLine";
        var multimediaLines = new List<MultimediaLine>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            multimediaLines.Add(new MultimediaLine
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                MultimediaId = reader.GetInt32(reader.GetOrdinal("MultimediaId")),
                LineNumber = reader.GetInt32(reader.GetOrdinal("LineNumber")),
                Text = reader.IsDBNull(reader.GetOrdinal("Text")) ? null : reader.GetString(reader.GetOrdinal("Text")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        return multimediaLines;
    }

    /// <summary>
    /// Retrieves a list of all multimedia lines from the database.
    /// </summary>
    /// <returns>A list of <see cref="MultimediaLine"/> objects.</returns>
    public async Task<List<MultimediaLine>> ListAsync(int multimediaId)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM MultimediaLine WHERE MultimediaId = @multimediaid";
        selectCmd.Parameters.AddWithValue("@multimediaid", multimediaId);
        var multimediaLines = new List<MultimediaLine>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            multimediaLines.Add(new MultimediaLine
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                MultimediaId = reader.GetInt32(reader.GetOrdinal("MultimediaId")),
                LineNumber = reader.GetInt32(reader.GetOrdinal("LineNumber")),
                Text = reader.IsDBNull(reader.GetOrdinal("Text")) ? null : reader.GetString(reader.GetOrdinal("Text")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        return multimediaLines;
    }

    /// <summary>
    /// Retrieves a specific multimedia line item by its ID.
    /// </summary>
    /// <param name="id">The ID of the multimedia line.</param>
    /// <returns>A <see cref="MultimediaLine"/> object if found; otherwise, null.</returns>
    public async Task<MultimediaLine?> GetAsync(int id)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM MultimediaLine WHERE Id = @id";
        selectCmd.Parameters.AddWithValue("@id", id);

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var multimediaLine = new MultimediaLine
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                MultimediaId = reader.GetInt32(reader.GetOrdinal("MultimediaId")),
                LineNumber = reader.GetInt32(reader.GetOrdinal("LineNumber")),
                Text = reader.IsDBNull(reader.GetOrdinal("Text")) ? null : reader.GetString(reader.GetOrdinal("Text")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            };

            return multimediaLine;
        }

        return null;
    }

    /// <summary>
    /// Saves a multimedia line item to the database. If the multimedia line Id is 0, a new multimedia item is created; otherwise, the existing multimedia line is updated.
    /// </summary>
    /// <param name="item">The multimedia line to save.</param>
    /// <returns>The Id of the saved multimedia line.</returns>
    public async Task<int> SaveItemAsync(MultimediaLine item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var saveCmd = connection.CreateCommand();
        if (item.Id == 0)
        {
            saveCmd.CommandText = @"
                INSERT INTO MultimediaLine (MultimediaId, LineNumber, Text, DateAdded, DateChanged)
                VALUES (@MultimediaId, @LineNumber, @Text, @DateAdded, @DateChanged);
                SELECT last_insert_rowid();";
            saveCmd.Parameters.AddWithValue("@DateAdded", DateTime.UtcNow);
        }
        else
        {
            saveCmd.CommandText = @"
                UPDATE MultimediaLine
                SET MultimediaId = @MultimediaId,
                    LineNumber = @LineNumber,
                    Text = @Text,
                    DateChanged = @DateChanged
                WHERE Id = @id";
            saveCmd.Parameters.AddWithValue("@id", item.Id);
        }

        saveCmd.Parameters.AddWithValue("@MultimediaId", item.MultimediaId);
        saveCmd.Parameters.AddWithValue("@LineNumber", item.LineNumber);
        saveCmd.Parameters.AddWithValue("@Text", item.Text);
        saveCmd.Parameters.AddWithValue("@DateChanged", DateTime.UtcNow);

        var result = await saveCmd.ExecuteScalarAsync();
        if (item.Id == 0)
        {
            item.Id = Convert.ToInt32(result);
        }

        return item.Id;
    }

    /// <summary>
    /// Deletes a multimedia line item from the database.
    /// </summary>
    /// <param name="item">The multimedia line to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(MultimediaLine item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM MultimediaLine WHERE Id = @id";
        deleteCmd.Parameters.AddWithValue("@id", item.Id);

        return await deleteCmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Deletes a multimedia line item from the database.
    /// </summary>
    /// <param name="multimediaId">The multimedia Id to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(int multimediaId)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM MultimediaLine WHERE MultimediaId = @multimediaid";
        deleteCmd.Parameters.AddWithValue("@multimediaid", multimediaId);

        return await deleteCmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Creates the MultimediaLine table in the database.
    /// </summary>
    public async Task CreateTableAsync()
    {
        await Init();
    }

    /// <summary>
    /// Drops the MultimediaLine table from the database.
    /// </summary>
    public async Task DropTableAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var dropCmd = connection.CreateCommand();
        dropCmd.CommandText = "DROP TABLE IF EXISTS MultimediaLine";
        await dropCmd.ExecuteNonQueryAsync();

        _hasBeenInitialized = false;
    }
}