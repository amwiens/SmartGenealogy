namespace SmartGenealogy.Data.Repositories;

/// <summary>
/// Repository class for managing Web Link link entities in the database.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="WebLinkLinkRepository"/> class.
/// </remarks>
/// <param name="databaseSettings">Database settings.</param>
/// <param name="logger">Logger.</param>
public class WebLinkLinkRepository(
    DatabaseSettings databaseSettings,
    WebLinkRepository webLinkRepository,
    ILogger<WebLinkLinkRepository> logger)
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
                @"CREATE TABLE IF NOT EXISTS WebLinkLink (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    WebLinkId INTEGER NOT NULL,
                    OwnerType INTEGER NOT NULL,
                    OwnerId INTEGER NOT NULL,
                    LinkType INTEGER NULL,
                    DateAdded TEXT NOT NULL,
                    DateChanged TEXT NOT NULL
                );";
            await createTableCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating WebLinkLink table.");
            throw;
        }

        // Initilization logic for the WebLink table would go here.
        _hasBeenInitialized = true;
    }

    /// <summary>
    /// Retrieves a list of all weblink links from the database.
    /// </summary>
    /// <returns>A list of <see cref="WebLinkLink"/> objects.</returns>
    public async Task<List<WebLinkLink>> ListAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM WebLinkLink";
        var webLinkLinks = new List<WebLinkLink>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            webLinkLinks.Add(new WebLinkLink
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                WebLinkId = reader.GetInt32(reader.GetOrdinal("WebLinkId")),
                OwnerType = (OwnerType)reader.GetInt32(reader.GetOrdinal("OwnerType")),
                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        foreach (var webLink in webLinkLinks)
        {
            webLink.WebLink = await webLinkRepository.GetAsync(webLink.WebLinkId);
        }

        return webLinkLinks;
    }

    /// <summary>
    /// Retrieves a list of all weblink links from the database.
    /// </summary>
    /// <param name="ownerType">Owner type.</param>
    /// <param name="ownerId">Owner Id</param>
    /// <returns>A list of <see cref="WebLinkLink"/> objects.</returns>
    public async Task<List<WebLinkLink>> ListAsync(OwnerType ownerType, int ownerId)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM WebLinkLink WHERE OwnerId = @ownerid AND OwnerType = @ownertype";
        selectCmd.Parameters.AddWithValue("@ownerid", ownerId);
        selectCmd.Parameters.AddWithValue("@ownertype", (int)ownerType);
        var webLinkLinks = new List<WebLinkLink>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            webLinkLinks.Add(new WebLinkLink
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                WebLinkId = reader.GetInt32(reader.GetOrdinal("WebLinkId")),
                OwnerType = (OwnerType)reader.GetInt32(reader.GetOrdinal("OwnerType")),
                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        foreach (var webLink in webLinkLinks)
        {
            webLink.WebLink = await webLinkRepository.GetAsync(webLink.WebLinkId);
        }

        return webLinkLinks;
    }

    /// <summary>
    /// Retrieves a specific weblink links by its ID.
    /// </summary>
    /// <param name="id">The ID of the web link.</param>
    /// <returns>A <see cref="WebLinkLink"/> object if found; otherwise, null.</returns>
    public async Task<WebLinkLink?> GetAsync(int id)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM WebLinkLink WHERE Id = @id";
        selectCmd.Parameters.AddWithValue("@id", id);

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var webLink = new WebLinkLink
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                WebLinkId = reader.GetInt32(reader.GetOrdinal("WebLinkId")),
                OwnerType = (OwnerType)reader.GetInt32(reader.GetOrdinal("OwnerType")),
                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            };

            webLink.WebLink = await webLinkRepository.GetAsync(webLink.WebLinkId);

            return webLink;
        }

        return null;
    }

    /// <summary>
    /// Saves a weblink link to the database. If the weblink link id is 0, a new weblink link is created; otherwise, the existing weblink link is updated.
    /// </summary>
    /// <param name="item">The weblink link to save.</param>
    /// <returns>The Id of the saved weblink link.</returns>
    public async Task<int> SaveItemAsync(WebLinkLink item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var saveCmd = connection.CreateCommand();
        if (item.Id == 0)
        {
            saveCmd.CommandText = @"
                INSERT INTO WebLinkLink (WebLinkId, OwnerType, OwnerId, DateAdded, DateChanged)
                VALUES (@WebLinkId, @OwnerType, @OwnerId, @DateAdded, @DateChanged);
                SELECT last_insert_rowid();";
            saveCmd.Parameters.AddWithValue("@DateAdded", DateTime.UtcNow);
        }
        else
        {
            saveCmd.CommandText = @"
                UPDATE WebLinkLink
                SET WebLinkId = @WebLinkId,
                    OwnerType = @OwnerType,
                    OwnerId = @OwnerId,
                    DateChanged = @DateChanged
                WHERE Id = @id";
            saveCmd.Parameters.AddWithValue("@id", item.Id);
        }

        saveCmd.Parameters.AddWithValue("@WebLinkId", item.WebLinkId);
        saveCmd.Parameters.AddWithValue("@OwnerType", item.OwnerType);
        saveCmd.Parameters.AddWithValue("@OwnerId", item.OwnerId);
        saveCmd.Parameters.AddWithValue("@DateChanged", DateTime.UtcNow);

        var result = await saveCmd.ExecuteScalarAsync();
        if (item.Id == 0)
        {
            item.Id = Convert.ToInt32(result);
        }

        return item.Id;
    }

    /// <summary>
    /// Deletes a weblink link from the database.
    /// </summary>
    /// <param name="item">The weblink link to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(WebLinkLink item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM WebLinkLink WHERE Id = @id";
        deleteCmd.Parameters.AddWithValue("@id", item.Id);

        return await deleteCmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Creates the WebLinkLink table in the database.
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
        dropCmd.CommandText = "DROP TABLE IF EXISTS WebLinkLink";
        await dropCmd.ExecuteNonQueryAsync();

        _hasBeenInitialized = false;
    }
}