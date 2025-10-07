namespace SmartGenealogy.Data.Repositories;

/// <summary>
/// Repository class for managing Role entities in the database.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RoleRepository"/> class.
/// </remarks>
/// <param name="databaseSettings">Database settings.</param>
/// <param name="logger">Logger.</param>
public class RoleRepository(DatabaseSettings databaseSettings, ILogger<RoleRepository> logger)
{
    private bool _hasBeenInitialized = false;

    /// <summary>
    /// Initializes the database connection and creates the Role table if it does not exist.
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
                @"CREATE TABLE IF NOT EXISTS Role (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    EventType INTEGER NOT NULL,
                    RoleType INTEGER NOT NULL,
                    Sentence TEXT NULL,
                    DateAdded TEXT NOT NULL,
                    DateChanged TEXT NOT NULL
                );";
            await createTableCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating Role table.");
            throw;
        }

        // Initialization logic for the Role table would go here.
        _hasBeenInitialized = true;
    }

    /// <summary>
    /// Retrieves a list of all roles from the database.
    /// </summary>
    /// <returns>A list of <see cref="Role"/> objects.</returns>
    public async Task<List<Role>> ListAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Role";
        var roles = new List<Role>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            roles.Add(new Role
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                EventType = reader.GetInt32(reader.GetOrdinal("EventType")),
                RoleType = reader.GetInt32(reader.GetOrdinal("RoleType")),
                Sentence = reader.IsDBNull(reader.GetOrdinal("Sentence")) ? null : reader.GetString(reader.GetOrdinal("Sentence")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        return roles;
    }

    /// <summary>
    /// Retrieves a list of roles by event type from the database.
    /// </summary>
    /// <param name="eventType">Event type.</param>
    /// <returns>A list of <see cref="Role"/> objects.</returns>
    public async Task<List<Role>> ListAsync(int eventType)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Role WHERE EventType = @eventType";
        selectCmd.Parameters.AddWithValue("@eventType", eventType);
        var roles = new List<Role>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            roles.Add(new Role
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                EventType = reader.GetInt32(reader.GetOrdinal("EventType")),
                RoleType = reader.GetInt32(reader.GetOrdinal("RoleType")),
                Sentence = reader.IsDBNull(reader.GetOrdinal("Sentence")) ? null : reader.GetString(reader.GetOrdinal("Sentence")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        return roles;
    }

    /// <summary>
    /// Retrieves a specific fact type by its ID.
    /// </summary>
    /// <param name="id">The ID of the role.</param>
    /// <returns>A <see cref="Role"/> object if found; otherwise, null.</returns>
    public async Task<Role?> GetAsync(int id)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Role WHERE Id = @id";
        selectCmd.Parameters.AddWithValue("@id", id);

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var role = new Role
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                EventType = reader.GetInt32(reader.GetOrdinal("EventType")),
                RoleType = reader.GetInt32(reader.GetOrdinal("RoleType")),
                Sentence = reader.IsDBNull(reader.GetOrdinal("Sentence")) ? null : reader.GetString(reader.GetOrdinal("Sentence")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            };

            return role;
        }

        return null;
    }

    /// <summary>
    /// Saves a role to the database. If the role id is 0, a new role is created; otherwise, the existing role is updated.
    /// </summary>
    /// <param name="item">The role to save.</param>
    /// <returns>The Id of the saved role.</returns>
    public async Task<int> SaveItemAsync(Role item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var saveCmd = connection.CreateCommand();
        if (item.Id == 0)
        {
            saveCmd.CommandText = @"
                INSERT INTO Role (Name, EventType, RoleType, Sentence, DateAdded, DateChanged)
                VALUES (@Name, @EventType, @RoleType, @Sentence, @DateAdded, @DateChanged);
                SELECT last_insert_rowid();";
            saveCmd.Parameters.AddWithValue("@DateAdded", DateTime.UtcNow);
        }
        else
        {
            saveCmd.CommandText = @"
                UPDATE Role
                SET Name = @Name,
                    EventType = @EventType,
                    RoleType = @RoleType,
                    Sentence = @Sentence,
                    DateChanged = @DateChanged
                WHERE Id = @id";
            saveCmd.Parameters.AddWithValue("@id", item.Id);
        }

        saveCmd.Parameters.AddWithValue("@Name", item.Name ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@EventType", item.EventType);
        saveCmd.Parameters.AddWithValue("@RoleType", item.RoleType);
        saveCmd.Parameters.AddWithValue("@Sentence", item.Sentence ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@DateChanged", DateTime.UtcNow);

        var result = await saveCmd.ExecuteScalarAsync();
        if (item.Id == 0)
        {
            item.Id = Convert.ToInt32(result);
        }

        return item.Id;
    }

    /// <summary>
    /// Deletes a role from the database.
    /// </summary>
    /// <param name="item">The role to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(Role item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM Role WHERE Id = @id";
        deleteCmd.Parameters.AddWithValue("@id", item.Id);

        return await deleteCmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Deletes roles from the database by fact type Id.
    /// </summary>
    /// <param name="factTypeId">Fact type Id.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(int factTypeId)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM Role WHERE EventType = @eventType";
        deleteCmd.Parameters.AddWithValue("@eventType", factTypeId);

        return await deleteCmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Creates the Role table in the database.
    /// </summary>
    public async Task CreateTableAsync()
    {
        await Init();
    }

    /// <summary>
    /// Drops the Role table from the database.
    /// </summary>
    public async Task DropTableAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var dropCmd = connection.CreateCommand();
        dropCmd.CommandText = "DROP TABLE IF EXISTS Role";
        await dropCmd.ExecuteNonQueryAsync();

        _hasBeenInitialized = false;
    }
}