using SmartGenealogy.Models;

using SQLite;

namespace SmartGenealogy.Services;

public class PlaceService
{
    private SQLiteAsyncConnection Database;

    public PlaceService()
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
        await Database.CreateTableAsync<Place>();
    }

    public async Task<Place?> GetPlaceAsync(int id)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.Table<Place>().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Place>> GetPlacesAsync()
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.Table<Place>().ToListAsync();
    }

    public async Task<int> AddPlaceAsync(Place place)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.InsertAsync(place);
    }

    public async Task<int> UpdatePlaceAsync(Place place)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.UpdateAsync(place);
    }

    public async Task<int> DeletePlaceAsync(Place place)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.DeleteAsync(place);
    }
}