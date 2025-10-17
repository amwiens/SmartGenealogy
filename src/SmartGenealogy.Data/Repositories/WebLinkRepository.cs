namespace SmartGenealogy.Data.Repositories;

/// <summary>
/// Repository class for managing Web Link entities in the database.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="WebLinkRepository"/> class.
/// </remarks>
/// <param name="databaseSettings">Database settings.</param>
/// <param name="logger">Logger.</param>
public class WebLinkRepository(DatabaseSettings databaseSettings, ILogger<WebLinkRepository> logger)
{
    private bool _hasBeenInitialized = false;

    /// <summary>
    /// Initializes the database connection and creates the WebLink table if it does not exist.
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
                @"CREATE TABLE IF NOT EXISTS WebLink (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    OwnerType INTEGER NOT NULL,
                    OwnerId INTEGER NOT NULL,
                    LinkType INTEGER NULL,
                    Name TEXT NOT NULL,
                    URL TEXT NOT NULL,
                    Note TEXT NULL,
                    DateAdded TEXT NOT NULL,
                    DateChanged TEXT NOT NULL
                );";
            await createTableCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating WebLink table.");
            throw;
        }

        // Initilization logic for the WebLink table would go here.
        _hasBeenInitialized = true;
    }

    /// <summary>
    /// Retrieves a list of all web links from the database.
    /// </summary>
    /// <returns>A list of <see cref="WebLink"/> objects.</returns>
    public async Task<List<WebLink>> ListAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM WebLink";
        var webLinks = new List<WebLink>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            webLinks.Add(new WebLink
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                OwnerType = (OwnerType)reader.GetInt32(reader.GetOrdinal("OwnerType")),
                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                LinkType = reader.GetInt32(reader.GetOrdinal("LinkType")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                URL = reader.GetString(reader.GetOrdinal("URL")),
                Note = reader.GetString(reader.GetOrdinal("Note")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        return webLinks;
    }

    /// <summary>
    /// Retrieves a list of all web links from the database.
    /// </summary>
    /// <param name="ownerType">Owner type.</param>
    /// <param name="ownerId">Owner Id</param>
    /// <returns>A list of <see cref="WebLink"/> objects.</returns>
    public async Task<List<WebLink>> ListAsync(OwnerType ownerType, int ownerId)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM WebLink WHERE OwnerId = @ownerid AND OwnerType = @ownertype";
        selectCmd.Parameters.AddWithValue("@ownerid", ownerId);
        selectCmd.Parameters.AddWithValue("@ownertype", (int)ownerType);
        var webLinks = new List<WebLink>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            webLinks.Add(new WebLink
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                OwnerType = (OwnerType)reader.GetInt32(reader.GetOrdinal("OwnerType")),
                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                LinkType = reader.GetInt32(reader.GetOrdinal("LinkType")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                URL = reader.GetString(reader.GetOrdinal("URL")),
                Note = reader.GetString(reader.GetOrdinal("Note")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        return webLinks;
    }

    /// <summary>
    /// Retrieves a specific web links by its ID.
    /// </summary>
    /// <param name="id">The ID of the web link.</param>
    /// <returns>A <see cref="WebLink"/> object if found; otherwise, null.</returns>
    public async Task<WebLink?> GetAsync(int id)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM WebLink WHERE Id = @id";
        selectCmd.Parameters.AddWithValue("@id", id);

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var webLink = new WebLink
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                OwnerType = (OwnerType)reader.GetInt32(reader.GetOrdinal("OwnerType")),
                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                LinkType = reader.GetInt32(reader.GetOrdinal("LinkType")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                URL = reader.GetString(reader.GetOrdinal("URL")),
                Note = reader.GetString(reader.GetOrdinal("Note")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            };

            return webLink;
        }

        return null;
    }

    /// <summary>
    /// Saves a web link to the database. If the web link id is 0, a new web link is created; otherwise, the existing web link is updated.
    /// </summary>
    /// <param name="item">The web link to save.</param>
    /// <returns>The Id of the saved web link.</returns>
    public async Task<int> SaveItemAsync(WebLink item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var saveCmd = connection.CreateCommand();
        if (item.Id == 0)
        {
            saveCmd.CommandText = @"
                INSERT INTO WebLink (OwnerType, OwnerId, LinkType, Name, URL, Note, DateAdded, DateChanged)
                VALUES (@OwnerType, @OwnerId, @LinkType, @Name, @URL, @Note, @DateAdded, @DateChanged);
                SELECT last_insert_rowid();";
            saveCmd.Parameters.AddWithValue("@DateAdded", DateTime.UtcNow);
        }
        else
        {
            saveCmd.CommandText = @"
                UPDATE WebLink
                SET OwnerType = @OwnerType,
                    OwnerId = @OwnerId,
                    LinkType = @LinkType,
                    Name = @Name,
                    URL = @URL,
                    Note = @Note,
                    DateChanged = @DateChanged
                WHERE Id = @id";
            saveCmd.Parameters.AddWithValue("@id", item.Id);
        }

        saveCmd.Parameters.AddWithValue("@OwnerType", item.OwnerType);
        saveCmd.Parameters.AddWithValue("@OwnerId", item.OwnerId);
        saveCmd.Parameters.AddWithValue("@LinkType", item.LinkType);
        saveCmd.Parameters.AddWithValue("@Name", item.Name);
        saveCmd.Parameters.AddWithValue("@URL", item.URL);
        saveCmd.Parameters.AddWithValue("@Note", item.Note);
        saveCmd.Parameters.AddWithValue("@DateChanged", DateTime.UtcNow);

        var result = await saveCmd.ExecuteScalarAsync();
        if (item.Id == 0)
        {
            item.Id = Convert.ToInt32(result);
        }

        return item.Id;
    }

    /// <summary>
    /// Deletes a web link from the database.
    /// </summary>
    /// <param name="item">The web link to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(WebLink item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM WebLink WHERE Id = @id";
        deleteCmd.Parameters.AddWithValue("@id", item.Id);

        return await deleteCmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Creates the WebLink table in the database.
    /// </summary>
    public async Task CreateTableAsync()
    {
        await Init();
    }

    /// <summary>
    /// Drops the WebLink table from the database.
    /// </summary>
    public async Task DropTableAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var dropCmd = connection.CreateCommand();
        dropCmd.CommandText = "DROP TABLE IF EXISTS WebLink";
        await dropCmd.ExecuteNonQueryAsync();

        _hasBeenInitialized = false;
    }
}