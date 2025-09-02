namespace SmartGenealogy.Data.Data;

public class PersonRepository
{
    private string _dbPath = Path.Combine(@"C:\Code\Mine\", "appdata.db");
    private string _dbSource;

    private bool _hasBeenInitialized = false;
    private readonly ILogger _logger;

    public PersonRepository(ILogger<PersonRepository> logger)
    {
        _logger = logger;

        _dbSource = $"DataSource={_dbPath};";
    }

    private async Task Init()
    {
        if (_hasBeenInitialized)
            return;

        await using var connection = new SqliteConnection(_dbSource);
        await connection.OpenAsync();

        try
        {
            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS Person (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Sex INTEGER NOT NULL,
                ParentID INTEGER NOT NULL
            );";
            await createTableCmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Person table");
            throw;
        }

        _hasBeenInitialized = true;
    }

    public async Task<List<Person>> ListAsync()
    {
        await Init();

        await using var connection = new SqliteConnection(_dbSource);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Person";
        var people = new List<Person>();



        return people;
    }

    public async Task<long> SaveItemAsync(Person item)
    {
        await Init();

        await using var connection = new SqliteConnection(_dbSource);
        await connection.OpenAsync();

        var saveCmd = connection.CreateCommand();
        if (item.Id == 0)
        {
            saveCmd.CommandText = @"
                INSERT INTO Person (Sex, ParentID)
                VALUES (@Sex, @ParentID);
                SELECT last_insert_rowid();";
        }
        else
        {
            saveCmd.CommandText = @"
                UPDATE Person SET Sex = @Sex, ParentID = @ParentID
                WHERE ID = @ID";
            saveCmd.Parameters.AddWithValue("@ID", item.Id);
        }

        saveCmd.Parameters.AddWithValue("@Sex", item.Sex);
        saveCmd.Parameters.AddWithValue("@ParentID", item.ParentID);

        var result = await saveCmd.ExecuteScalarAsync();
        if (item.Id == 0)
        {
            item.Id = Convert.ToInt64(result);
        }

        return item.Id;
    }
}