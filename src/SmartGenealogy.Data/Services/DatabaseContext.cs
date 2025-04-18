using System.Linq.Expressions;

namespace SmartGenealogy.Data.Services;

public class DatabaseContext : IAsyncDisposable
{
    public string? DatabasePath { get; set; }

    private SQLiteAsyncConnection? _connection;
    private SQLiteAsyncConnection? Database;

    private void InitializeDatabase()
    {
        if (string.IsNullOrEmpty(DatabasePath))
            throw new InvalidOperationException("Database path is not set.");
        Database = (_connection = new SQLiteAsyncConnection(DatabasePath,
            SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache));
    }

    private async Task CreateTableIfNotExists<T>() where T : class, new()
    {
        if (Database is null)
            InitializeDatabase();
        await Database!.CreateTableAsync<T>();
    }

    private async Task<AsyncTableQuery<T>> GetTableAsync<T>() where T : class, new()
    {
        if (Database is null)
            InitializeDatabase();
        await CreateTableIfNotExists<T>();
        return Database!.Table<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class, new()
    {
        if (Database is null)
            InitializeDatabase();
        var table = await GetTableAsync<T>();
        return await table.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetFilteredAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new()
    {
        if (Database is null)
            InitializeDatabase();
        var table = await GetTableAsync<T>();
        return await table.Where(predicate).ToListAsync();
    }

    private async Task<TResult> Execute<T, TResult>(Func<Task<TResult>> action) where T : class, new()
    {
        if (Database is null)
            InitializeDatabase();
        await CreateTableIfNotExists<T>();
        return await action();
    }

    public async Task<T> GetItemByKeyAsync<T>(object primaryKey) where T : class, new()
    {
        if (Database is null)
            InitializeDatabase();
        return await Execute<T, T>(async () => await Database!.GetAsync<T>(primaryKey));
    }

    public async Task<bool> AddItemAsync<T>(T item) where T : class, new()
    {
        if (Database is null)
            InitializeDatabase();
        return await Execute<T, bool>(async () => await Database!.InsertAsync(item) > 0);
    }

    public async Task<bool> UpdateItemAsync<T>(T item) where T : class, new()
    {
        if (Database is null)
            InitializeDatabase();
        await CreateTableIfNotExists<T>();
        return await Database!.UpdateAsync(item) > 0;
    }

    public async Task<bool> DeleteItemAsync<T>(T item) where T : class, new()
    {
        if (Database is null)
            InitializeDatabase();
        await CreateTableIfNotExists<T>();
        return await Database!.DeleteAsync(item) > 0;
    }

    public async Task<bool> DeleteItemByKeyAsync<T>(object primaryKey) where T : class, new()
    {
        if (Database is null)
            InitializeDatabase();
        await CreateTableIfNotExists<T>();
        return await Database!.DeleteAsync<T>(primaryKey) > 0;
    }

    public async ValueTask DisposeAsync() => await _connection?.CloseAsync();
}