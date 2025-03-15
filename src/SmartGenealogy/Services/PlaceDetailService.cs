using SmartGenealogy.Models;

using SQLite;

namespace SmartGenealogy.Services;

public class PlaceDetailService
{
    SQLiteAsyncConnection Database;

    public PlaceDetailService()
    {
        var databasePath = Path.Combine(AppContext.BaseDirectory, "SmartGenealogy.db");
        //var databasePath = Path.Combine(FileSystem.AppDataDirectory, "SmartGenealogy.db");
        Database = new SQLiteAsyncConnection(databasePath);
    }

    bool isInitialized;
    SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

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

    async Task Init()
    {
        await Database.CreateTableAsync<PlaceDetail>();
    }

    public async Task<PlaceDetail?> GetPlaceDetailAsync(int id)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.Table<PlaceDetail>().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<PlaceDetail>> GetPlaceDetailsByPlaceIdAsync(int placeId)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.Table<PlaceDetail>().Where(p => p.PlaceId == placeId).ToListAsync();
    }

    public async Task<List<PlaceDetail>> GetPlaceDetailsAsync()
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.Table<PlaceDetail>().ToListAsync();
    }

    public async Task<int> AddPlaceDetailAsync(PlaceDetail place)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.InsertAsync(place);
    }

    public async Task<int> UpdatePlaceDetailAsync(PlaceDetail place)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.UpdateAsync(place);
    }

    public async Task<int> DeletePlaceDetailAsync(PlaceDetail place)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.DeleteAsync(place);
    }
}