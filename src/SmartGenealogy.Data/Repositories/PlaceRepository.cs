namespace SmartGenealogy.Data.Repositories;

/// <summary>
/// Repository class for managing Place entities in the database.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PlaceRepository"/> class.
/// </remarks>
/// <param name="databaseSettings">Database settings.</param>
/// <param name="logger">Logger.</param>
public class PlaceRepository(DatabaseSettings databaseSettings, ILogger<PlaceRepository> logger)
{
    private bool _hasBeenInitialized = false;

    /// <summary>
    /// Initializes the database connection and creates the Place table if it does not exist.
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
                @"CREATE TABLE IF NOT EXISTS Place (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PlaceType INTEGER NOT NULL,
                    Name TEXT NOT NULL,
                    Abbreviation TEXT NULL,
                    Normalized TEXT NULL,
                    Latitude REAL NULL,
                    Longitude REAL NULL,
                    MasterId INTEGER NOT NULL,
                    Note TEXT NULL,
                    Reverse TEXT NOT NULL,
                    DateAdded TEXT NOT NULL,
                    DateChanged TEXT NOT NULL
                );";
            await createTableCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating Place table.");
            throw;
        }

        // Initizliation logic for the Place table would go here.
        _hasBeenInitialized = true;
    }

    /// <summary>
    /// Retrieves a list of all places from the database.
    /// </summary>
    /// <returns>A list of <see cref="Place"/> objects.</returns>
    public async Task<List<Place>> ListAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Place";
        var places = new List<Place>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            places.Add(new Place
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                PlaceType = (PlaceType)reader.GetInt64(reader.GetOrdinal("PlaceType")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                Abbreviation = reader.IsDBNull(reader.GetOrdinal("Abbreviation")) ? null : reader.GetString(reader.GetOrdinal("Abbreviation")),
                Normalized = reader.IsDBNull(reader.GetOrdinal("Normalized")) ? null : reader.GetString(reader.GetOrdinal("Normalized")),
                Latitude = reader.GetDecimal(reader.GetOrdinal("Latitude")),
                Longitude = reader.GetDecimal(reader.GetOrdinal("Longitude")),
                MasterId = reader.GetInt32(reader.GetOrdinal("MasterId")),
                Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? null : reader.GetString(reader.GetOrdinal("Note")),
                Reverse = reader.IsDBNull(reader.GetOrdinal("Reverse")) ? null : reader.GetString(reader.GetOrdinal("Reverse")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        foreach (var place in places)
        {
            place.PlaceDetails = await ListAsync(place.Id);
        }

        return places;
    }

    /// <summary>
    /// Retrieves a list of all places from the database.
    /// </summary>
    /// <param name="masterId">Master Id</param>
    /// <returns>A list of <see cref="Place"/> objects.</returns>
    public async Task<List<Place>> ListAsync(int masterId)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Place WHERE MasterId = @masterid";
        selectCmd.Parameters.AddWithValue("@masterid", masterId);
        var places = new List<Place>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            places.Add(new Place
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                PlaceType = (PlaceType)reader.GetInt64(reader.GetOrdinal("PlaceType")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                Abbreviation = reader.IsDBNull(reader.GetOrdinal("Abbreviation")) ? null : reader.GetString(reader.GetOrdinal("Abbreviation")),
                Normalized = reader.IsDBNull(reader.GetOrdinal("Normalized")) ? null : reader.GetString(reader.GetOrdinal("Normalized")),
                Latitude = reader.GetDecimal(reader.GetOrdinal("Latitude")),
                Longitude = reader.GetDecimal(reader.GetOrdinal("Longitude")),
                MasterId = reader.GetInt32(reader.GetOrdinal("MasterId")),
                Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? null : reader.GetString(reader.GetOrdinal("Note")),
                Reverse = reader.IsDBNull(reader.GetOrdinal("Reverse")) ? null : reader.GetString(reader.GetOrdinal("Reverse")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        return places;
    }

    /// <summary>
    /// Retrieves a specific place by its ID.
    /// </summary>
    /// <param name="id">The ID of the place.</param>
    /// <returns>A <see cref="Place"/> object if found; otherwise, null.</returns>
    public async Task<Place?> GetAsync(int id)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Place WHERE Id = @id";
        selectCmd.Parameters.AddWithValue("@id", id);

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var place = new Place
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                PlaceType = (PlaceType)reader.GetInt64(reader.GetOrdinal("PlaceType")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                Abbreviation = reader.IsDBNull(reader.GetOrdinal("Abbreviation")) ? null : reader.GetString(reader.GetOrdinal("Abbreviation")),
                Normalized = reader.IsDBNull(reader.GetOrdinal("Normalized")) ? null : reader.GetString(reader.GetOrdinal("Normalized")),
                Latitude = reader.GetDecimal(reader.GetOrdinal("Latitude")),
                Longitude = reader.GetDecimal(reader.GetOrdinal("Longitude")),
                MasterId = reader.GetInt32(reader.GetOrdinal("MasterId")),
                Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? null : reader.GetString(reader.GetOrdinal("Note")),
                Reverse = reader.IsDBNull(reader.GetOrdinal("Reverse")) ? null : reader.GetString(reader.GetOrdinal("Reverse")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            };

            place.PlaceDetails = await ListAsync(place.Id);

            return place;
        }

        return null;
    }

    /// <summary>
    /// Saves a place to the database. If the place Id is 0, a new place is created; otherwise, the existing place is updated.
    /// </summary>
    /// <param name="item">The place to save.</param>
    /// <returns>The Id of the saved place.</returns>
    public async Task<int> SaveItemAsync(Place item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var saveCmd = connection.CreateCommand();
        if (item.Id == 0)
        {
            saveCmd.CommandText = @"
                INSERT INTO Place (PlaceType, Name, Abbreviation, Normalized, Latitude, Longitude, MasterId, Note, Reverse, DateAdded, DateChanged)
                VALUES (@PlaceType, @Name, @Abbreviation, @Normalized, @Latitude, @Longitude, @MasterId, @Note, @Reverse, @DateAdded, @DateChanged);
                SELECT last_insert_rowid();";
            saveCmd.Parameters.AddWithValue("@DateAdded", DateTime.UtcNow);
        }
        else
        {
            saveCmd.CommandText = @"
                UPDATE Place
                SET PlaceType = @PlaceType,
                    Name = @Name,
                    Abbreviation = @Abbreviation,
                    Normalized = @Normalized,
                    Latitude = @Latitude,
                    Longitude = @Longitude,
                    MasterId = @MasterId,
                    Note = @Note,
                    Reverse = @Reverse,
                    DateChanged = @DateChanged
                WHERE Id = @id";
            saveCmd.Parameters.AddWithValue("@id", item.Id);
        }

        saveCmd.Parameters.AddWithValue("@PlaceType", item.PlaceType);
        saveCmd.Parameters.AddWithValue("@Name", item.Name ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@Abbreviation", item.Abbreviation ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@Normalized", item.Normalized ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@Latitude", item.Latitude);
        saveCmd.Parameters.AddWithValue("@Longitude", item.Longitude);
        saveCmd.Parameters.AddWithValue("@MasterId", item.MasterId);
        saveCmd.Parameters.AddWithValue("@Note", item.Note ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@Reverse", item.Reverse ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@DateChanged", DateTime.UtcNow);

        var result = await saveCmd.ExecuteScalarAsync();
        if (item.Id == 0)
        {
            item.Id = Convert.ToInt32(result);
        }

        return item.Id;
    }

    /// <summary>
    /// Deletes a place from the database.
    /// </summary>
    /// <param name="item">The place to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(Place item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM Place WHERE Id = @id";
        deleteCmd.Parameters.AddWithValue("@id", item.Id);

        return await deleteCmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Creates the Place table in the database.
    /// </summary>
    public async Task CreateTableAsync()
    {
        await Init();
    }

    /// <summary>
    /// Drops the Place table from the database.
    /// </summary>
    public async Task DropTableAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var dropCmd = connection.CreateCommand();
        dropCmd.CommandText = "DROP TABLE IF EXISTS Place";
        await dropCmd.ExecuteNonQueryAsync();

        _hasBeenInitialized = false;
    }
}