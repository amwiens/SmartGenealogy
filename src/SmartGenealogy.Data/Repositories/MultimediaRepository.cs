namespace SmartGenealogy.Data.Repositories;

/// <summary>
/// Repository class for managing Multimedia entities in the database.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MultimediaRepository"/> class.
/// </remarks>
/// <param name="databaseSettings">Database settings.</param>
/// <param name="logger">Logger.</param>
public class MultimediaRepository(
    DatabaseSettings databaseSettings,
    ILogger<MultimediaRepository> logger)
{
    private bool _hasBeenInitialized = false;

    /// <summary>
    /// Initializes the database connection and creates the Multimedia table if it does not exist.
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
                @"CREATE TABLE IF NOT EXISTS Multimedia(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    MediaType INTEGER NOT NULL,
                    MediaPath TEXT NOT NULL,
                    MediaFile TEXT NOT NULL,
                    Url TEXT NULL,
                    Thumbnail BLOB NULL,
                    Caption TEXT NULL,
                    RefNumber TEXT NULL,
                    Date TEXT NULL,
                    SortDate INTEGER NOT NULL,
                    Description TEXT NULL,
                    AllText TEXT NULL,
                    DateAdded TEXT NOT NULL,
                    DateChanged TEXT NOT NULL
                );";
            await createTableCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating Multimedia table.");
            throw;
        }

        // Initialization logic for the Multimedia table would go here.
        _hasBeenInitialized = true;
    }

    /// <summary>
    /// Retrieves a list of all multimedia from the database.
    /// </summary>
    /// <returns>A list of <see cref="Multimedia"/> objects.</returns>
    public async Task<List<Multimedia>> ListAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Multimedia";
        var multimedia = new List<Multimedia>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            multimedia.Add(new Multimedia
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                MediaType = (MediaType)reader.GetInt64(reader.GetOrdinal("MediaType")),
                MediaPath = reader.IsDBNull(reader.GetOrdinal("MediaPath")) ? null : reader.GetString(reader.GetOrdinal("MediaPath")),
                MediaFile = reader.IsDBNull(reader.GetOrdinal("MediaFile")) ? null : reader.GetString(reader.GetOrdinal("MediaFile")),
                Url = reader.IsDBNull(reader.GetOrdinal("Url")) ? null : reader.GetString(reader.GetOrdinal("Url")),
                Thumbnail = reader.IsDBNull(reader.GetOrdinal("Thumbnail")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("Thumbnail")),
                Caption = reader.IsDBNull(reader.GetOrdinal("Caption")) ? null : reader.GetString(reader.GetOrdinal("Caption")),
                RefNumber = reader.IsDBNull(reader.GetOrdinal("RefNumber")) ? null : reader.GetString(reader.GetOrdinal("RefNumber")),
                Date = reader.IsDBNull(reader.GetOrdinal("Date")) ? null : reader.GetString(reader.GetOrdinal("Date")),
                SortDate = reader.GetInt32(reader.GetOrdinal("SortDate")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                AllText = reader.IsDBNull(reader.GetOrdinal("AllText")) ? null : reader.GetString(reader.GetOrdinal("AllText")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        return multimedia;
    }

    /// <summary>
    /// Retrieves a specific multimedia item by its ID.
    /// </summary>
    /// <param name="id">The ID of the fact type.</param>
    /// <returns>A <see cref="Multimedia"/> object if found; otherwise, null.</returns>
    public async Task<Multimedia?> GetAsync(int id)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Multimedia WHERE Id = @id";
        selectCmd.Parameters.AddWithValue("@id", id);

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var multimedia = new Multimedia
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                MediaType = (MediaType)reader.GetInt64(reader.GetOrdinal("MediaType")),
                MediaPath = reader.IsDBNull(reader.GetOrdinal("MediaPath")) ? null : reader.GetString(reader.GetOrdinal("MediaPath")),
                MediaFile = reader.IsDBNull(reader.GetOrdinal("MediaFile")) ? null : reader.GetString(reader.GetOrdinal("MediaFile")),
                Url = reader.IsDBNull(reader.GetOrdinal("Url")) ? null : reader.GetString(reader.GetOrdinal("Url")),
                Thumbnail = reader.IsDBNull(reader.GetOrdinal("Thumbnail")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("Thumbnail")),
                Caption = reader.IsDBNull(reader.GetOrdinal("Caption")) ? null : reader.GetString(reader.GetOrdinal("Caption")),
                RefNumber = reader.IsDBNull(reader.GetOrdinal("RefNumber")) ? null : reader.GetString(reader.GetOrdinal("RefNumber")),
                Date = reader.IsDBNull(reader.GetOrdinal("Date")) ? null : reader.GetString(reader.GetOrdinal("Date")),
                SortDate = reader.GetInt32(reader.GetOrdinal("SortDate")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                AllText = reader.IsDBNull(reader.GetOrdinal("AllText")) ? null : reader.GetString(reader.GetOrdinal("AllText")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            };

            return multimedia;
        }

        return null;
    }

    /// <summary>
    /// Saves a multimedia item to the database. If the multimedia Id is 0, a new multimedia item is created; otherwise, the existing multimedia is updated.
    /// </summary>
    /// <param name="item">The multimedia to save.</param>
    /// <returns>The Id of the saved multimedia.</returns>
    public async Task<int> SaveItemAsync(Multimedia item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var saveCmd = connection.CreateCommand();
        if (item.Id == 0)
        {
            saveCmd.CommandText = @"
                INSERT INTO Multimedia (MediaType, MediaPath, MediaFile, Url, Thumbnail, Caption, RefNumber, Date, SortDate, Description, AllText, DateAdded, DateChanged)
                VALUES (@MediaType, @MediaPath, @MediaFile, @Url, @Thumbnail, @Caption, @RefNumber, @Date, @SortDate, @Description, @AllText, @DateAdded, @DateChanged);
                SELECT last_insert_rowid();";
            saveCmd.Parameters.AddWithValue("@DateAdded", DateTime.UtcNow);
        }
        else
        {
            saveCmd.CommandText = @"
                UPDATE Multimedia
                SET MediaType = @MediaType,
                    MediaPath = @MediaPath,
                    MediaFile = @MediaFile,
                    Url = @Url,
                    Thumbnail = @Thumbnail,
                    Caption = @Caption,
                    RefNumber = @RefNumber,
                    Date = @Date,
                    SortDate = @SortDate,
                    Description = @Description,
                    AllText = @AllText,
                    DateChanged = @DateChanged
                WHERE Id = @id";
            saveCmd.Parameters.AddWithValue("@id", item.Id);
        }

        saveCmd.Parameters.AddWithValue("@MediaType", item.MediaType);
        saveCmd.Parameters.AddWithValue("@MediaPath", item.MediaPath ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@MediaFile", item.MediaFile ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@Url", item.Url ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@Thumbnail", (object?)item.Thumbnail ?? DBNull.Value);
        saveCmd.Parameters.AddWithValue("@Caption", item.Caption ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@RefNumber", item.RefNumber ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@Date", item.Date ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@SortDate", item.SortDate);
        saveCmd.Parameters.AddWithValue("@Description", item.Description ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@AllText", item.AllText ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@DateChanged", DateTime.UtcNow);

        var result = await saveCmd.ExecuteScalarAsync();
        if (item.Id == 0)
        {
            item.Id = Convert.ToInt32(result);
        }

        return item.Id;
    }

    /// <summary>
    /// Deletes a multimedia item from the database.
    /// </summary>
    /// <param name="item">The multimedia to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(Multimedia item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM Multimedia WHERE Id = @id";
        deleteCmd.Parameters.AddWithValue("@id", item.Id);

        return await deleteCmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Creates the Multimedia table in the database.
    /// </summary>
    public async Task CreateTableAsync()
    {
        await Init();
    }

    /// <summary>
    /// Drops the Multimedia table from the database.
    /// </summary>
    public async Task DropTableAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var dropCmd = connection.CreateCommand();
        dropCmd.CommandText = "DROP TABLE IF EXISTS Multimedia";
        await dropCmd.ExecuteNonQueryAsync();

        _hasBeenInitialized = false;
    }
}