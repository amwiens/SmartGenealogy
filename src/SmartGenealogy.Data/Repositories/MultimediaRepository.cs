namespace SmartGenealogy.Data.Repositories;

/// <summary>
/// Repository class for managing multimedia in the database.
/// </summary>
public class MultimediaRepository
{
    private readonly DatabaseSettings _databaseSettings;

    private bool _hasBeenInitialized = false;
    private readonly ILogger<MultimediaRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MultimediaRepository"/> class.
    /// </summary>
    /// <param name="databaseSettings"></param>
    /// <param name="logger"></param>
    public MultimediaRepository(DatabaseSettings databaseSettings, ILogger<MultimediaRepository> logger)
    {
        _databaseSettings = databaseSettings;
        _logger = logger;
    }

    /// <summary>
    /// Initializes the database connection and creates the Multimedia table if it does not exist.
    /// </summary>
    private async Task Init()
    {
        if (_hasBeenInitialized)
            return;

        await using var connection = new SqliteConnection(_databaseSettings.ConnectionString);
        await connection.OpenAsync();

        try
        {
            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS Multimedia (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    MediaType INTEGER NOT NULL,
                    MediaPath TEXT NOT NULL,
                    MediaFile TEXT NOT NULL,
                    URL TEXT NULL,
                    Thumbnail BLOB NULL,
                    Caption TEXT NULL,
                    RefNumber TEXT NULL,
                    Date TEXT NULL,
                    SortDate INTEGER NOT NULL,
                    Description TEXT NULL,
                    DateAdded TEXT NOT NULL,
                    DateChanged TEXT NOT NULL
                );";
            await createTableCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing Multimedia table.");
            throw;
        }

        // Initialization logic for the Multimedia table goes here.
        _hasBeenInitialized = true;
    }

    /// <summary>
    /// Retrieves a list of all multimedia from the database.
    /// </summary>
    /// <returns>A list of <see cref="Multimedia"/> objects.</returns>
    public async Task<List<Multimedia>> ListAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(_databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Multimedia";
        var multimedia = new List<Multimedia>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var item = new Multimedia
            {
                Id = reader.GetInt64(reader.GetOrdinal("Id")),
                MediaType = (MediaType)reader.GetInt64(reader.GetOrdinal("MediaType")),
                MediaPath = reader.IsDBNull(reader.GetOrdinal("MediaPath")) ? null : reader.GetString(reader.GetOrdinal("MediaPath")),
                MediaFile = reader.IsDBNull(reader.GetOrdinal("MediaFile")) ? null : reader.GetString(reader.GetOrdinal("MediaFile")),
                URL = reader.IsDBNull(reader.GetOrdinal("URL")) ? null : reader.GetString(reader.GetOrdinal("URL")),
                Thumbnail = reader.IsDBNull(reader.GetOrdinal("Thumbnail")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("Thumbnail")),
                Caption = reader.IsDBNull(reader.GetOrdinal("Caption")) ? null : reader.GetString(reader.GetOrdinal("Caption")),
                RefNumber = reader.IsDBNull(reader.GetOrdinal("RefNumber")) ? null : reader.GetString(reader.GetOrdinal("RefNumber")),
                Date = reader.IsDBNull(reader.GetOrdinal("Date")) ? null : reader.GetString(reader.GetOrdinal("Date")),
                SortDate = reader.GetInt64(reader.GetOrdinal("SortDate")),
                Description = reader.IsDBNull(reader.GetOrdinal("MediaType")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            };
            multimedia.Add(item);
        }

        return multimedia;
    }

    /// <summary>
    /// Retrieves a list of all multimedia associated with a specific media type.
    /// </summary>
    /// <param name="mediaType">The ID of the media type.</param>
    /// <returns>A list of <see cref="Multimedia"/> objects.</returns>
    public async Task<List<Multimedia>> ListAsync(int mediaType)
    {
        await Init();
        await using var connection = new SqliteConnection(_databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Multimedia WHERE MediaType = @mediaType";
        selectCmd.Parameters.AddWithValue("@mediaType", mediaType);
        var multimedia = new List<Multimedia>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var item = new Multimedia
            {
                Id = reader.GetInt64(reader.GetOrdinal("Id")),
                MediaType = (MediaType)reader.GetInt64(reader.GetOrdinal("MediaType")),
                MediaPath = reader.IsDBNull(reader.GetOrdinal("MediaPath")) ? null : reader.GetString(reader.GetOrdinal("MediaPath")),
                MediaFile = reader.IsDBNull(reader.GetOrdinal("MediaFile")) ? null : reader.GetString(reader.GetOrdinal("MediaFile")),
                URL = reader.IsDBNull(reader.GetOrdinal("URL")) ? null : reader.GetString(reader.GetOrdinal("URL")),
                Thumbnail = reader.IsDBNull(reader.GetOrdinal("Thumbnail")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("Thumbnail")),
                Caption = reader.IsDBNull(reader.GetOrdinal("Caption")) ? null : reader.GetString(reader.GetOrdinal("Caption")),
                RefNumber = reader.IsDBNull(reader.GetOrdinal("RefNumber")) ? null : reader.GetString(reader.GetOrdinal("RefNumber")),
                Date = reader.IsDBNull(reader.GetOrdinal("Date")) ? null : reader.GetString(reader.GetOrdinal("Date")),
                SortDate = reader.GetInt64(reader.GetOrdinal("SortDate")),
                Description = reader.IsDBNull(reader.GetOrdinal("MediaType")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            };
            multimedia.Add(item);
        }

        return multimedia;
    }

    /// <summary>
    /// Retrieves a specific multimedia item by its Id.
    /// </summary>
    /// <param name="id">The Id of the multimedia item.</param>
    /// <returns>A <see cref="Multimedia"/> object if found; otherwise, null.</returns>
    public async Task<Multimedia?> GetAsync(long id)
    {
        await Init();
        await using var connection = new SqliteConnection(_databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Multimedia WHERE Id = @id";
        selectCmd.Parameters.AddWithValue("@id", id);

        await using var reader = await selectCmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var multimedia = new Multimedia
            {
                Id = reader.GetInt64(reader.GetOrdinal("Id")),
                MediaType = (MediaType)reader.GetInt64(reader.GetOrdinal("MediaType")),
                MediaPath = reader.IsDBNull(reader.GetOrdinal("MediaPath")) ? null : reader.GetString(reader.GetOrdinal("MediaPath")),
                MediaFile = reader.IsDBNull(reader.GetOrdinal("MediaFile")) ? null : reader.GetString(reader.GetOrdinal("MediaFile")),
                URL = reader.IsDBNull(reader.GetOrdinal("URL")) ? null : reader.GetString(reader.GetOrdinal("URL")),
                Thumbnail = reader.IsDBNull(reader.GetOrdinal("Thumbnail")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("Thumbnail")),
                Caption = reader.IsDBNull(reader.GetOrdinal("Caption")) ? null : reader.GetString(reader.GetOrdinal("Caption")),
                RefNumber = reader.IsDBNull(reader.GetOrdinal("RefNumber")) ? null : reader.GetString(reader.GetOrdinal("RefNumber")),
                Date = reader.IsDBNull(reader.GetOrdinal("Date")) ? null : reader.GetString(reader.GetOrdinal("Date")),
                SortDate = reader.GetInt64(reader.GetOrdinal("SortDate")),
                Description = reader.IsDBNull(reader.GetOrdinal("MediaType")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            };

            return multimedia;
        }
        return null;
    }

    /// <summary>
    /// Saves a multimedia item to the database. If the multimedia item has an Id of 0, it will be inserted as a new record.
    /// </summary>
    /// <param name="item">The multimedia item to save.</param>
    /// <returns>The Id of the saved fact type.</returns>
    public async Task<long> SaveItemAsync(Multimedia item)
    {
        await Init();
        await using var connection = new SqliteConnection(_databaseSettings.ConnectionString);
        await connection.OpenAsync();
        var cmd = connection.CreateCommand();
        if (item.Id == 0)
        {
            cmd.CommandText = @"
                INSERT INTO Multimedia (MediaType, MediaPath, MediaFile, URL, Thumbnail, Caption, RefNumber, Date, SortDate, Description, DateAdded, DateChanged)
                VALUES (@mediaType, @mediaPath, @mediaFile, @url, @thumbnail, @caption, @refNumber, @date, @sortDate, @description, @dateAdded, @dateChanged);
                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@dateAdded", DateTime.UtcNow);
        }
        else
        {
            cmd.CommandText = @"
                UPDATE Multimedia
                SET MediaType = @mediaType,
                    MediaPath = @mediaPath,
                    MediaFile = @mediaFile,
                    URL = @url,
                    Thumbnail = @thumbnail,
                    Caption = @caption,
                    RefNumber = @refNumber,
                    Date = @date,
                    SortDate = @sortDate,
                    Description = @description,
                    DateChanged = @dateChanged
                WHERE Id = @Id;";
            cmd.Parameters.AddWithValue("@Id", item.Id);
        }

        cmd.Parameters.AddWithValue("@mediaType", item.MediaType);
        cmd.Parameters.AddWithValue("@mediaPath", item.MediaPath ?? string.Empty);
        cmd.Parameters.AddWithValue("@mediaFile", item.MediaFile ?? string.Empty);
        cmd.Parameters.AddWithValue("@url", item.URL ?? string.Empty);
        cmd.Parameters.AddWithValue("@thumbnail", (object?)item.Thumbnail ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@caption", item.Caption ?? string.Empty);
        cmd.Parameters.AddWithValue("@refNumber", item.RefNumber ?? string.Empty);
        cmd.Parameters.AddWithValue("@date", item.Date ?? string.Empty);
        cmd.Parameters.AddWithValue("@sortDate", item.SortDate);
        cmd.Parameters.AddWithValue("@description", item.Description ?? string.Empty);
        cmd.Parameters.AddWithValue("@dateChanged", DateTime.UtcNow);

        var result = await cmd.ExecuteScalarAsync();
        if (item.Id == 0)
        {
            item.Id = Convert.ToInt64(result);
        }

        return item.Id;
    }

    /// <summary>
    /// Deletes a multimedia item from the database.
    /// </summary>
    /// <param name="id">The multimedia item to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(long id)
    {
        await Init();
        await using var connection = new SqliteConnection(_databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM Multimedia WHERE Id = @id";
        cmd.Parameters.AddWithValue("@id", id);

        return await cmd.ExecuteNonQueryAsync();
    }
}