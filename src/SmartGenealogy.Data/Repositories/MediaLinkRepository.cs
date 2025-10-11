namespace SmartGenealogy.Data.Repositories;

/// <summary>
/// Repository class for managing Media link entities in the database.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediaLinkRepository"/> class.
/// </remarks>
/// <param name="databaseSettings">Database settings.</param>
/// <param name="logger">Logger.</param>
public class MediaLinkRepository(
    DatabaseSettings databaseSettings,
    ILogger<MediaLinkRepository> logger)
{
    private bool _hasBeenInitialized = false;

    /// <summary>
    /// Initializes the database connection and creates the Media link table if it does not exist.
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
                @"CREATE TABLE IF NOT EXISTS MediaLink(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    MultimediaId INTEGER NOT NULL,
                    OwnerType INTEGER NOT NULL,
                    OwnerId INTEGER NOT NULL,
                    IsPrimary INTEGER NULL,
                    Thumbnail BLOB NULL,
                    Comments TEXT NULL,
                    DateAdded TEXT NOT NULL,
                    DateChanged TEXT NOT NULL
                );";
            await createTableCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating Media Link table.");
            throw;
        }

        // Initialization logic for the Media link table would go here.
        _hasBeenInitialized = true;
    }

    /// <summary>
    /// Retrieves a list of all media links from the database.
    /// </summary>
    /// <returns>A list of <see cref="MediaLink"/> objects.</returns>
    public async Task<List<MediaLink>> ListAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM MediaLink";
        var mediaLinks = new List<MediaLink>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            mediaLinks.Add(new MediaLink
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                MultimediaId = reader.GetInt32(reader.GetOrdinal("MultimediaId")),
                OwnerType = (OwnerType)reader.GetInt32(reader.GetOrdinal("OwnerType")),
                OwnerId = reader.GetInt32(reader.GetOrdinal("MediaFile")),
                IsPrimary = bool.Parse(reader.GetString(reader.GetOrdinal("IsPrimary"))),
                Comments = reader.IsDBNull(reader.GetOrdinal("Comments")) ? null : reader.GetString(reader.GetOrdinal("Comments")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        return mediaLinks;
    }

    /// <summary>
    /// Retrieves a list of all media links for an owner type and id from the database.
    /// </summary>
    /// <param name="ownerType">The owner type.</param>
    /// <param name="ownerId">The owner id.</param>
    /// <returns>A list of <see cref="MediaLink"/> objects.</returns>
    public async Task<List<MediaLink>?> ListAsync(OwnerType ownerType, int ownerId)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM MediaLink WHERE OwnerType = @ownertype and OwnerId = @ownerid";
        selectCmd.Parameters.AddWithValue("@ownertype", ownerType);
        selectCmd.Parameters.AddWithValue("@ownerid ", ownerId);
        var mediaLinks = new List<MediaLink>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            mediaLinks.Add(new MediaLink
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                MultimediaId = reader.GetInt32(reader.GetOrdinal("MultimediaId")),
                OwnerType = (OwnerType)reader.GetInt32(reader.GetOrdinal("OwnerType")),
                OwnerId = reader.GetInt32(reader.GetOrdinal("MediaFile")),
                IsPrimary = bool.Parse(reader.GetString(reader.GetOrdinal("IsPrimary"))),
                Comments = reader.IsDBNull(reader.GetOrdinal("Comments")) ? null : reader.GetString(reader.GetOrdinal("Comments")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        return mediaLinks;
    }

    /// <summary>
    /// Retrieves a specific media link item by its ID.
    /// </summary>
    /// <param name="id">The ID of the fact type.</param>
    /// <returns>A <see cref="MediaLink"/> object if found; otherwise, null.</returns>
    public async Task<MediaLink?> GetAsync(int id)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM MediaLink WHERE Id = @id";
        selectCmd.Parameters.AddWithValue("@id", id);

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var mediaLink = new MediaLink
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                MultimediaId = reader.GetInt32(reader.GetOrdinal("MultimediaId")),
                OwnerType = (OwnerType)reader.GetInt32(reader.GetOrdinal("OwnerType")),
                OwnerId = reader.GetInt32(reader.GetOrdinal("MediaFile")),
                IsPrimary = bool.Parse(reader.GetString(reader.GetOrdinal("IsPrimary"))),
                Comments = reader.IsDBNull(reader.GetOrdinal("Comments")) ? null : reader.GetString(reader.GetOrdinal("Comments")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            };

            return mediaLink;
        }

        return null;
    }

    /// <summary>
    /// Saves a media link item to the database. If the media link Id is 0, a new media link item is created; otherwise, the existing media link is updated.
    /// </summary>
    /// <param name="item">The media link to save.</param>
    /// <returns>The Id of the saved media link.</returns>
    public async Task<int> SaveItemAsync(MediaLink item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var saveCmd = connection.CreateCommand();
        if (item.Id == 0)
        {
            saveCmd.CommandText = @"
                INSERT INTO MediaLink (MultimediaId, OwnerType, OwnerId, IsPrimary, Comments, DateAdded, DateChanged)
                VALUES (@MultimediaId, @OwnerType, @OwnerId, @IsPrimary, @Comments, @DateAdded, @DateChanged);
                SELECT last_insert_rowid();";
            saveCmd.Parameters.AddWithValue("@DateAdded", DateTime.UtcNow);
        }
        else
        {
            saveCmd.CommandText = @"
                UPDATE MediaLink
                SET MultimediaId = @MultimediaId,
                    OwnerType = @OwnerType,
                    OwnerId = @OwnerId,
                    IsPrimary = @IsPrimary,
                    Comments = @Comments,
                    DateChanged = @DateChanged
                WHERE Id = @id";
            saveCmd.Parameters.AddWithValue("@id", item.Id);
        }

        saveCmd.Parameters.AddWithValue("@MultimediaId", item.MultimediaId);
        saveCmd.Parameters.AddWithValue("@OwnerType", item.OwnerType);
        saveCmd.Parameters.AddWithValue("@OwnerId", item.OwnerId);
        saveCmd.Parameters.AddWithValue("@IsPrimary", item.IsPrimary);
        saveCmd.Parameters.AddWithValue("@Comments", item.Comments ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@DateChanged", DateTime.UtcNow);

        var result = await saveCmd.ExecuteScalarAsync();
        if (item.Id == 0)
        {
            item.Id = Convert.ToInt32(result);
        }

        return item.Id;
    }

    /// <summary>
    /// Deletes a media link item from the database.
    /// </summary>
    /// <param name="item">The media link to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(Multimedia item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM MediaLink WHERE Id = @id";
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
    /// Drops the Media link table from the database.
    /// </summary>
    public async Task DropTableAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var dropCmd = connection.CreateCommand();
        dropCmd.CommandText = "DROP TABLE IF EXISTS MediaLink";
        await dropCmd.ExecuteNonQueryAsync();

        _hasBeenInitialized = false;
    }
}