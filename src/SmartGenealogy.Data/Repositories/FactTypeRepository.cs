namespace SmartGenealogy.Data.Repositories;

/// <summary>
/// Repository class for managing FactType entities in the database.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FactTypeRepository"/> class.
/// </remarks>
/// <param name="roleRepository">Role repository.</param>
/// <param name="databaseSettings">Database settings.</param>
/// <param name="logger">Logger.</param>
public class FactTypeRepository(RoleRepository roleRepository, DatabaseSettings databaseSettings, ILogger<FactTypeRepository> logger)
{
    private bool _hasBeenInitialized = false;

    /// <summary>
    /// Initializes the database connection and creates the FactType table if it does not exist.
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
                @"CREATE TABLE IF NOT EXISTS FactType (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    OwnerType INTEGER NOT NULL,
                    Name TEXT NOT NULL,
                    Abbreviation TEXT NULL,
                    GedcomTag TEXT NULL,
                    UseValue INTEGER NOT NULL,
                    UseDate INTEGER NOT NULL,
                    UsePlace INTEGER NOT NULL,
                    Sentence TEXT NULL,
                    IsBuiltIn INTEGER NOT NULL,
                    DateAdded TEXT NOT NULL,
                    DateChanged TEXT NOT NULL
                );";
            await createTableCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating FactType table.");
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
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM FactType";
        var factTypes = new List<FactType>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            factTypes.Add(new FactType
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                OwnerType = (OwnerType)reader.GetInt64(reader.GetOrdinal("OwnerType")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                Abbreviation = reader.IsDBNull(reader.GetOrdinal("Abbreviation")) ? null : reader.GetString(reader.GetOrdinal("Abbreviation")),
                GedcomTag = reader.IsDBNull(reader.GetOrdinal("GedcomTag")) ? null : reader.GetString(reader.GetOrdinal("GedcomTag")),
                UseValue = reader.GetBoolean(reader.GetOrdinal("UseValue")),
                UseDate = reader.GetBoolean(reader.GetOrdinal("UseDate")),
                UsePlace = reader.GetBoolean(reader.GetOrdinal("UsePlace")),
                Sentence = reader.IsDBNull(reader.GetOrdinal("Sentence")) ? null : reader.GetString(reader.GetOrdinal("Sentence")),
                IsBuiltIn = reader.GetBoolean(reader.GetOrdinal("IsBuiltIn")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        foreach (var factType in factTypes)
        {
            factType.Roles = await roleRepository.ListAsync(factType.Id);
        }

        return factTypes;
    }

    /// <summary>
    /// Retrieves a specific fact type by its ID.
    /// </summary>
    /// <param name="id">The ID of the fact type.</param>
    /// <returns>A <see cref="FactType"/> object if found; otherwise, null.</returns>
    public async Task<FactType?> GetAsync(int id)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM FactType WHERE Id = @id";
        selectCmd.Parameters.AddWithValue("@id", id);

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var factType = new FactType
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                OwnerType = (OwnerType)reader.GetInt64(reader.GetOrdinal("OwnerType")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                Abbreviation = reader.IsDBNull(reader.GetOrdinal("Abbreviation")) ? null : reader.GetString(reader.GetOrdinal("Abbreviation")),
                GedcomTag = reader.IsDBNull(reader.GetOrdinal("GedcomTag")) ? null : reader.GetString(reader.GetOrdinal("GedcomTag")),
                UseValue = reader.GetBoolean(reader.GetOrdinal("UseValue")),
                UseDate = reader.GetBoolean(reader.GetOrdinal("UseDate")),
                UsePlace = reader.GetBoolean(reader.GetOrdinal("UsePlace")),
                Sentence = reader.IsDBNull(reader.GetOrdinal("Sentence")) ? null : reader.GetString(reader.GetOrdinal("Sentence")),
                IsBuiltIn = reader.GetBoolean(reader.GetOrdinal("IsBuiltIn")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            };

            factType.Roles = await roleRepository.ListAsync(factType.Id);

            return factType;
        }

        return null;
    }

    /// <summary>
    /// Saves a fact type to the database. If the fact type Id is 0, a new fact type is created; otherwise, the existing fact type is updated.
    /// </summary>
    /// <param name="item">The fact type to save.</param>
    /// <returns>The Id of the saved fact type.</returns>
    public async Task<int> SaveItemAsync(FactType item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var saveCmd = connection.CreateCommand();
        if (item.Id == 0)
        {
            saveCmd.CommandText = @"
                INSERT INTO FactType (OwnerType, Name, Abbreviation, GedcomTag, UseValue, UseDate, UsePlace, Sentence, IsBuiltIn, DateAdded, DateChanged)
                VALUES (@OwnerType, @Name, @Abbreviation, @GedcomTag, @UseValue, @UseDate, @UsePlace, @Sentence, @IsBuiltIn, @DateAdded, @DateChanged);
                SELECT last_insert_rowid();";
            saveCmd.Parameters.AddWithValue("@DateAdded", DateTime.UtcNow);
        }
        else
        {
            saveCmd.CommandText = @"
                UPDATE FactType
                SET OwnerType = @OwnerType,
                    Name = @Name,
                    Abbreviation = @Abbreviation,
                    GedcomTag = @GedcomTag,
                    UseValue = @UseValue,
                    UseDate = @UseDate,
                    UsePlace = @UsePlace,
                    Sentence = @Sentence,
                    IsBuiltIn = @IsBuiltIn,
                    DateChanged = @DateChanged
                WHERE Id = @id";
            saveCmd.Parameters.AddWithValue("@id", item.Id);
        }

        saveCmd.Parameters.AddWithValue("@OwnerType", item.OwnerType);
        saveCmd.Parameters.AddWithValue("@Name", item.Name ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@Abbreviation", item.Abbreviation ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@GedcomTag", item.GedcomTag ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@UseValue", item.UseValue ? 1 : 0);
        saveCmd.Parameters.AddWithValue("@UseDate", item.UseDate ? 1 : 0);
        saveCmd.Parameters.AddWithValue("@UsePlace", item.UsePlace ? 1 : 0);
        saveCmd.Parameters.AddWithValue("@Sentence", item.Sentence ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@IsBuiltIn", item.IsBuiltIn ? 1 : 0);
        saveCmd.Parameters.AddWithValue("@DateChanged", DateTime.UtcNow);

        var result = await saveCmd.ExecuteScalarAsync();
        if (item.Id == 0)
        {
            item.Id = Convert.ToInt32(result);
        }

        return item.Id;
    }

    /// <summary>
    /// Deletes a fact type from the database.
    /// </summary>
    /// <param name="item">The fact type to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(FactType item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM FactType WHERE Id = @id";
        deleteCmd.Parameters.AddWithValue("@id", item.Id);

        return await deleteCmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Creates the FactType table in the database.
    /// </summary>
    public async Task CreateTableAsync()
    {
        await Init();
    }

    /// <summary>
    /// Drops the FactType table from the database.
    /// </summary>
    public async Task DropTableAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var dropCmd = connection.CreateCommand();
        dropCmd.CommandText = "DROP TABLE IF EXISTS FactType";
        await dropCmd.ExecuteNonQueryAsync();

        _hasBeenInitialized = false;
    }
}