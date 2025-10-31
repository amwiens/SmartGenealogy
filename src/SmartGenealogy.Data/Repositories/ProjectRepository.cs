namespace SmartGenealogy.Data.Repositories;

/// <summary>
/// Repository class for managing Project entities in the database.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ProjectRepository"/> class.
/// </remarks>
/// <param name="databaseSettings">Database settings.</param>
/// <param name="logger">Logger</param>
public class ProjectRepository(
    DatabaseSettings databaseSettings,
    ILogger<ProjectRepository> logger)
{
    private bool _hasBeenInitialized = false;

    /// <summary>
    /// INitializes the database connection and creates the Project table if it does not exist.
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
                @"CREATE TABLE IF NOT EXISTS Project (
                    Id TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Description TEXT,
                    Priority INTEGER,
                    Status INTEGER,
                    StartDate TEXT,
                    EndDate TEXT,
                    DateAdded TEXT NOT NULL,
                    DateChanged TEXT NOT NULL
                );";
            await createTableCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating Project table.");
            throw;
        }

        // Initialization logic for the Project table would go here.
        _hasBeenInitialized = true;
    }

    /// <summary>
    /// Retrieves a list of all projects from the database.
    /// </summary>
    /// <returns>A list of <see cref="Project"/> objects.</returns>
    public async Task<List<Project>> ListAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Project";
        var projects = new List<Project>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            projects.Add(new Project
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Priority = reader.IsDBNull(reader.GetOrdinal("Priority")) ? null : (Priority?)reader.GetInt32(reader.GetOrdinal("Priority")),
                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : (ProjectStatus?)reader.GetInt32(reader.GetOrdinal("Status")),
                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            });
        }

        return projects;
    }

    /// <summary>
    /// Retrieves a specific project by its ID.
    /// </summary>
    /// <returns>A <see cref="Project"/> objectif found; otherwise, null.</returns>
    public async Task<Project?> GetAsync(int id)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Project WHERE Id = @id";
        selectCmd.Parameters.AddWithValue("@id", id);

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var project = new Project
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Priority = reader.IsDBNull(reader.GetOrdinal("Priority")) ? null : (Priority?)reader.GetInt32(reader.GetOrdinal("Priority")),
                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : (ProjectStatus?)reader.GetInt32(reader.GetOrdinal("Status")),
                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged"))
            };

            return project;
        }

        return null;
    }

    /// <summary>
    /// Saves a project to the database. If the project Id is 0, a new project is created; otherwise, the existing project is updated.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<int> SaveItemAsync(Project item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var saveCmd = connection.CreateCommand();
        if (item.Id == 0)
        {
            saveCmd.CommandText = @"
                INSERT INTO Project (Name, Description, Priority, Status, StartDate, EndDate, DateAdded, DateChanged)
                VALUES (@Name, @Description, @Priority, @Status, @StartDate, @EndDate, @DateAdded, @DateChanged);
                SELECT last_insert_rowid();";
            saveCmd.Parameters.AddWithValue("@DateAdded", DateTime.UtcNow);
        }
        else
        {
            saveCmd.CommandText = @"
                UPDATE Project
                SET Name = @Name,
                    Description = @Description,
                    Priority = @Priority,
                    Status = @Status,
                    StartDate = @StartDate,
                    EndDate = @EndDate,
                    DateChanged = @DateChanged
                WHERE Id = @Id";
            saveCmd.Parameters.AddWithValue("@Id", item.Id);
        }

        saveCmd.Parameters.AddWithValue("@Name", item.Name ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@Description", item.Description ?? string.Empty);
        saveCmd.Parameters.AddWithValue("@Priority", item.Priority.HasValue ? (int)item.Priority.Value : (object)DBNull.Value);
        saveCmd.Parameters.AddWithValue("@Status", item.Status.HasValue ? (int)item.Status.Value : (object)DBNull.Value);
        saveCmd.Parameters.AddWithValue("@StartDate", item.StartDate);
        saveCmd.Parameters.AddWithValue("@EndDate", item.EndDate);
        saveCmd.Parameters.AddWithValue("@DateChanged", DateTime.UtcNow);

        var result = await saveCmd.ExecuteScalarAsync();
        if (item.Id == 0)
        {
            item.Id = Convert.ToInt32(result);
        }

        return item.Id;
    }

    /// <summary>
    /// Deletes a project from the database.
    /// </summary>
    /// <param name="item">The project to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteItemAsync(Project item)
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM Project WHERE Id = @Id";
        deleteCmd.Parameters.AddWithValue("@Id", item.Id);

        return await deleteCmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Creates the Project table in the database.
    /// </summary>
    public async Task CreateTableAsync()
    {
        await Init();
    }

    /// <summary>
    /// Drops the Project table from the database
    /// </summary>
    public async Task DropTableAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        var dropCmd = connection.CreateCommand();
        dropCmd.CommandText = "DROP TABLE IF EXISTS Project";
        await dropCmd.ExecuteNonQueryAsync();

        _hasBeenInitialized = false;
    }
}