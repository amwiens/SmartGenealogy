namespace SmartGenealogy.Data.Repositories;

/// <summary>
/// Repository class for managing Multimedia word entities in the database.
/// </summary>
/// <param name="databaseSettings">Database settings</param>
/// <param name="logger">Logger</param>
public class MultimediaWordRepository(
    DatabaseSettings databaseSettings,
    ILogger<MultimediaLineRepository> logger)
{
    private bool _hasBeenInitialized = false;

    /// <summary>
    /// Initializes the database connection and creates the Multimedia word table if it does not exist.
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
                @"CREATE TABLE IF NOT EXISTS MultimediaWord(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    MultimediaId INTEGER NOT NULL,
                    Confidence REAL NOT NULL,
                    Height INTEGER NOT NULL,
                    Text TEXT NOT NULL,
                    Width INTEGER NOT NULL,
                    X INTEGER NOT NULL,
                    Y INTEGER NOT NULL,
                    DateAdded TEXT NOT NULL,
                    DateChanged TEXT NOT NULL
                );";
            await createTableCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating Multimedia word table.");
            throw;
        }

        // Initialization logic for the Multimedia word table would go here.
        _hasBeenInitialized = true;
    }

    /// <summary>
    /// Retrieves a list of all multimedia words from the database.
    /// </summary>
    /// <returns>A list of <see cref="MultimediaWord"/> objects.</returns>
    public async Task<List<MultimediaWord>> ListAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM MultimediaWord";
        var multimediaWords = new List<MultimediaWord>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            multimediaWords.Add(new MultimediaWord
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                MultimediaId = reader.GetInt32(reader.GetOrdinal("MultimediaId")),
                Confidence = (float)reader.GetDecimal(reader.GetOrdinal("Confidence")),
                Height = reader.GetInt32(reader.GetOrdinal("Height")),
                Text = reader.IsDBNull(reader.GetOrdinal("Text")) ? null : reader.GetString(reader.GetOrdinal("Text")),
                Width = reader.GetInt32(reader.GetOrdinal("Width")),
                X = reader.GetInt32(reader.GetOrdinal("X")),
                Y = reader.GetInt32(reader.GetOrdinal("Y")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        return multimediaWords;
    }


    /// <summary>
    /// Retrieves a list of all multimedia words from the database.
    /// </summary>
    /// <returns>A list of <see cref="MultimediaWord"/> objects.</returns>
    public async Task<List<MultimediaWord>> ListAsync(int multimediaId)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM MultimediaWord WHERE MultimediaId = @multimediaid";
        selectCmd.Parameters.AddWithValue("@multimediaid", multimediaId);
        var multimediaWords = new List<MultimediaWord>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            multimediaWords.Add(new MultimediaWord
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                MultimediaId = reader.GetInt32(reader.GetOrdinal("MultimediaId")),
                Confidence = (float)reader.GetDecimal(reader.GetOrdinal("Confidence")),
                Height = reader.GetInt32(reader.GetOrdinal("Height")),
                Text = reader.IsDBNull(reader.GetOrdinal("Text")) ? null : reader.GetString(reader.GetOrdinal("Text")),
                Width = reader.GetInt32(reader.GetOrdinal("Width")),
                X = reader.GetInt32(reader.GetOrdinal("X")),
                Y = reader.GetInt32(reader.GetOrdinal("Y")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        return multimediaWords;
    }

    /// <summary>
    /// Retrieves a specific multimedia word item by its ID.
    /// </summary>
    /// <param name="id">The ID of the multimedia word.</param>
    /// <returnsA <see cref="MultimediaWord"/> object if found; otherwise, null.></returns>
    public async Task<MultimediaWord?> GetAsync(int id)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM MultimediaWord WHERE Id = @id";
        selectCmd.Parameters.AddWithValue("@id", id);

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var multimediaWord = new MultimediaWord
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                MultimediaId = reader.GetInt32(reader.GetOrdinal("MultimediaId")),
                Confidence = (float)reader.GetDecimal(reader.GetOrdinal("Confidence")),
                Height = reader.GetInt32(reader.GetOrdinal("Height")),
                Text = reader.IsDBNull(reader.GetOrdinal("Text")) ? null : reader.GetString(reader.GetOrdinal("Text")),
                Width = reader.GetInt32(reader.GetOrdinal("Width")),
                X = reader.GetInt32(reader.GetOrdinal("X")),
                Y = reader.GetInt32(reader.GetOrdinal("Y")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            };

            return multimediaWord;
        }

        return null;
    }

    /// <summary>
    /// Saves a multimedia word item to the database. If the multimedia word Id is 0, a new multimedia word item is created; otherwise, the existing multimedia word is updated.
    /// </summary>
    /// <param name="item">The multimedia word to save.</param>
    /// <returns>The Id of the saved multimedia word.</returns>
    public async Task<int> SaveItemAsync(MultimediaWord item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var saveCmd = connection.CreateCommand();
        if (item.Id == 0)
        {
            saveCmd.CommandText = @"
                INSERT INTO MultimediaWord (MultimediaId, Confidence, Height, Text, Width, X, Y, DateAdded, DateChanged)
                VALUES (@MultimediaId, @Confidence, @Height, @Text, @Width, @X, @Y, @DateAdded, @DateChanged);
                SELECT last_insert_rowid();";
            saveCmd.Parameters.AddWithValue("@DateAdded", DateTime.UtcNow);
        }
        else
        {
            saveCmd.CommandText = @"
                UPDATE MultimediaWord
                SET MultimediaId = @MultimediaId,
                    Confidence = @Confidence,
                    Height = @Height,
                    Text = @Text,
                    Width = @Width,
                    X = @X,
                    Y = @Y,
                    DateChanged = @DateChanged
                WHERE Id = @id";
            saveCmd.Parameters.AddWithValue("@id", item.Id);
        }

        saveCmd.Parameters.AddWithValue("@MultimediaId", item.MultimediaId);
        saveCmd.Parameters.AddWithValue("@Confidence", item.Confidence);
        saveCmd.Parameters.AddWithValue("@Height", item.Height);
        saveCmd.Parameters.AddWithValue("@Text", item.Text);
        saveCmd.Parameters.AddWithValue("@Width", item.Width);
        saveCmd.Parameters.AddWithValue("@X", item.X);
        saveCmd.Parameters.AddWithValue("@Y", item.Y);
        saveCmd.Parameters.AddWithValue("@DateChanged", DateTime.UtcNow);

        var result = await saveCmd.ExecuteScalarAsync();
        if (item.Id == 0)
        {
            item.Id = Convert.ToInt32(result);
        }

        return item.Id;
    }

    /// <summary>
    /// Deletes a multimedia word item from the database.
    /// </summary>
    /// <param name="item">The multimedia word to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(MultimediaWord item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM MultimediaWord WHERE Id = @id";
        deleteCmd.Parameters.AddWithValue("@id", item.Id);

        return await deleteCmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Deletes a multimedia word item from the database.
    /// </summary>
    /// <param name="multimediaId">The multimedia Id to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(int multimediaId)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM MultimediaWord WHERE MultimediaId = @multimediaid";
        deleteCmd.Parameters.AddWithValue("@multimediaid", multimediaId);

        return await deleteCmd.ExecuteNonQueryAsync();
    }


    /// <summary>
    /// Creates the MultimediaWord table in the database.
    /// </summary>
    public async Task CreateTableAsync()
    {
        await Init();
    }

    /// <summary>
    /// Drops the MultimediaWord table from the database.
    /// </summary>
    public async Task DropTableAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var dropCmd = connection.CreateCommand();
        dropCmd.CommandText = "DROP TABLE IF EXISTS MultimediaWord";
        await dropCmd.ExecuteNonQueryAsync();

        _hasBeenInitialized = false;
    }
}