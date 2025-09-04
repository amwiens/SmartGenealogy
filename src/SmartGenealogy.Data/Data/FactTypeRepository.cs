namespace SmartGenealogy.Data.Data;

/// <summary>
/// Repository class for managing fact types in the database.
/// </summary>
public class FactTypeRepository
{
    private readonly DatabaseSettings _databaseSettings;

    private bool _hasBeenInitialized = false;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instanc of the <see cref="FactTypeRepository"/> class.
    /// </summary>
    /// <param name="logger"></param>
    public FactTypeRepository(DatabaseSettings databaseSettings, ILogger<FactTypeRepository> logger)
    {
        _databaseSettings = databaseSettings;
        _logger = logger;
    }

    /// <summary>
    /// Initializes the database connection and creates the FactType table if it does not exist.
    /// </summary>
    private async Task Init()
    {
        if (_hasBeenInitialized)
            return;

        await using var connection = new SqliteConnection(_databaseSettings.ConnectionString);
        await connection.OpenAsync();

        try
        {
            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS FactType (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                OwnerType INTEGER NOT NULL,
                Name TEXT NOT NULL,
                Abbreviation TEXT NULL,
                GedcomTag TEXT NULL,
                UseValue INTEGER NOT NULL,
                UseDate INTEGER NOT NULL,
                UsePlace INTEGER NOT NULL,
                Sentence TEXT NULL,
                DateCreated TEXT NOT NULL,
                DateUpdated TEXT NOT NULL
            );";
            await createTableCmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating FactType table");
            throw;
        }

        // Initialization logic for the FactType table would go here.
        _hasBeenInitialized = true;
    }

    /// <summary>
    /// Retrieves a list of all fact types from the database.
    /// </summary>
    /// <returns>A list of <see cref="FactType"/> objects.</returns>
    public async Task<List<FactType>> ListAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(_databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM FactType";
        var factTypes = new List<FactType>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var factType = new FactType
            {
                Id = reader.GetInt64(0),
                OwnerType = (OwnerType)reader.GetInt64(1),
                Name = reader.IsDBNull(2) ? null : reader.GetString(2),
                Abbreviation = reader.IsDBNull(3) ? null : reader.GetString(3),
                GedcomTag = reader.IsDBNull(4) ? null : reader.GetString(4),
                UseValue = reader.GetBoolean(5),
                UseDate = reader.GetBoolean(6),
                UsePlace = reader.GetBoolean(7),
                Sentence = reader.IsDBNull(8) ? null : reader.GetString(8),
                DateCreated = reader.GetDateTime(9),
                DateUpdated = reader.GetDateTime(10)
            };
            factTypes.Add(factType);
        }

        return factTypes;
    }

    /// <summary>
    /// Retrieves a list of fact types associated with a specific owner type.
    /// </summary>
    /// <param name="ownerType">The ID of the owner type.</param>
    /// <returns>A list of <see cref="FactType"/> objects.</returns>
    public async Task<List<FactType>> ListAsync(int ownerType)
    {
        await Init();
        await using var connection = new SqliteConnection(_databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM FactType WHERE OwnerType = @ownerType";
        selectCmd.Parameters.AddWithValue("@ownerType", ownerType);
        var factTypes = new List<FactType>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var factType = new FactType
            {
                Id = reader.GetInt64(0),
                OwnerType = (OwnerType)reader.GetInt64(1),
                Name = reader.IsDBNull(2) ? null : reader.GetString(2),
                Abbreviation = reader.IsDBNull(3) ? null : reader.GetString(3),
                GedcomTag = reader.IsDBNull(4) ? null : reader.GetString(4),
                UseValue = reader.GetBoolean(5),
                UseDate = reader.GetBoolean(6),
                UsePlace = reader.GetBoolean(7),
                Sentence = reader.IsDBNull(8) ? null : reader.GetString(8),
                DateCreated = reader.GetDateTime(9),
                DateUpdated = reader.GetDateTime(10)
            };
            factTypes.Add(factType);
        }

        return factTypes;
    }

    /// <summary>
    /// Retrieves a specific task by its Id.
    /// </summary>
    /// <param name="id">The Id of the fact type.</param>
    /// <returns> a <see cref="FactType"/> object if found; otherwise, null.</returns>
    public async Task<FactType?> GetAsync(long id)
    {
        await Init();
        await using var connection = new SqliteConnection(_databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM FactType WHERE Id = @id";
        selectCmd.Parameters.AddWithValue("@id", id);

        await using var reader = await selectCmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var factType = new FactType
            {
                Id = reader.GetInt64(0),
                OwnerType = (OwnerType)reader.GetInt64(1),
                Name = reader.IsDBNull(2) ? null : reader.GetString(2),
                Abbreviation = reader.IsDBNull(3) ? null : reader.GetString(3),
                GedcomTag = reader.IsDBNull(4) ? null : reader.GetString(4),
                UseValue = reader.GetBoolean(5),
                UseDate = reader.GetBoolean(6),
                UsePlace = reader.GetBoolean(7),
                Sentence = reader.IsDBNull(8) ? null : reader.GetString(8),
                DateCreated = reader.GetDateTime(9),
                DateUpdated = reader.GetDateTime(10)
            };

            return factType;
        }

        return null;
    }

    /// <summary>
    /// Saves a fact type to the database. If the fact type has an Id of 0, it will be inserted as a new record.
    /// </summary>
    /// <param name="item">The fact type to save.</param>
    /// <returns>The Id of the saved fact type.</returns>
    public async Task<long> SaveItemAsync(FactType item)
    {
        await Init();
        await using var connection = new SqliteConnection(_databaseSettings.ConnectionString);
        await connection.OpenAsync();
        var cmd = connection.CreateCommand();
        if (item.Id == 0)
        {
            cmd.CommandText = @"
            INSERT INTO FactType (OwnerType, Name, Abbreviation, GedcomTag, UseValue, UseDate, UsePlace, Sentence, DateCreated, DateUpdated)
            VALUES (@OwnerType, @Name, @Abbreviation, @GedcomTag, @UseValue, @UseDate, @UsePlace, @Sentence, @DateCreated, @DateUpdated);
            SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@DateCreated", DateTime.UtcNow);
        }
        else
        {
            cmd.CommandText = @"
            UPDATE FactType
            SET OwnerType = @OwnerType,
                Name = @Name,
                Abbreviation = @Abbreviation,
                GedcomTag = @GedcomTag,
                UseValue = @UseValue,
                UseDate = @UseDate,
                UsePlace = @UsePlace,
                Sentence = @Sentence,
                DateUpdated = @DateUpdated
            WHERE Id = @Id;";
            cmd.Parameters.AddWithValue("@Id", item.Id);
        }

        cmd.Parameters.AddWithValue("@OwnerType", item.OwnerType);
        cmd.Parameters.AddWithValue("@Name", item.Name ?? string.Empty);
        cmd.Parameters.AddWithValue("@Abbreviation", item.Abbreviation ?? string.Empty);
        cmd.Parameters.AddWithValue("@GedcomTag", item.GedcomTag ?? string.Empty);
        cmd.Parameters.AddWithValue("@UseValue", item.UseValue ? 1 : 0);
        cmd.Parameters.AddWithValue("@UseDate", item.UseDate ? 1 : 0);
        cmd.Parameters.AddWithValue("@UsePlace", item.UsePlace ? 1 : 0);
        cmd.Parameters.AddWithValue("@Sentence", item.Sentence ?? string.Empty);
        cmd.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);

        var result = await cmd.ExecuteScalarAsync();
        if (item.Id == 0)
        {
            item.Id = Convert.ToInt64(result);
        }

        return item.Id;
    }

    /// <summary>
    /// Deletes a fact type from the database.
    /// </summary>
    /// <param name="id">The fact type to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(long id)
    {
        await Init();
        await using var connection = new SqliteConnection(_databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM FactType WHERE Id = @id";
        deleteCmd.Parameters.AddWithValue("@id", id);

        return await deleteCmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Drops the FactType table from the database.
    /// </summary>
    public async Task DropTableAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(_databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var dropTableCmd = connection.CreateCommand();
        dropTableCmd.CommandText = "DROP TABLE IF EXISTS FactType";
        await dropTableCmd.ExecuteNonQueryAsync();
        _hasBeenInitialized = false;
    }
}