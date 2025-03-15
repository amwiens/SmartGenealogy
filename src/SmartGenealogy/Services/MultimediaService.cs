using SmartGenealogy.Models;

using SQLite;

namespace SmartGenealogy.Services;

public class MultimediaService
{
    private SQLiteAsyncConnection Database;

    public MultimediaService()
    {
        var databasePath = Path.Combine(AppContext.BaseDirectory, "SmartGenealogy.db");
        //var databasePath = Path.Combine(FileSystem.AppDataDirectory, "SmartGenealogy.db");
        Database = new SQLiteAsyncConnection(databasePath);
    }

    private bool isInitialized;
    private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

    public async Task InitializeAsync()
    {
        await semaphoreSlim.WaitAsync();
        try
        {
            if (!isInitialized)
            {
                await Init();
                isInitialized = true;
            }
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task Init()
    {
        await Database.CreateTableAsync<Multimedia>();
    }

    public async Task<Multimedia?> GetMultimediaAsync(int id)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.Table<Multimedia>().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Multimedia>> GetMultimediaAsync()
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.Table<Multimedia>().ToListAsync();
    }

    public async Task<int> AddMultimediaAsync(Multimedia multimedia)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.InsertAsync(multimedia);
    }

    public async Task<int> UpdateMultimediaAsync(Multimedia multimedia)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.UpdateAsync(multimedia);
    }

    public async Task<int> DeleteMultimediaAsync(Multimedia multimedia)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.DeleteAsync(multimedia);
    }
}