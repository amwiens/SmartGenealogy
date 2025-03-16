using SmartGenealogy.Models;

using SQLite;

namespace SmartGenealogy.Services;

public class MediaLinkService
{
    private SQLiteAsyncConnection Database;

    public MediaLinkService()
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
        await Database.CreateTableAsync<MediaLink>();
    }

    public async Task<MediaLink?> GetMediaLinkAsync(int id)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.Table<MediaLink>().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<MediaLink>> GetMultimediaAsync()
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.Table<MediaLink>().ToListAsync();
    }

    public async Task<int> AddMediaLinkAsync(MediaLink mediaLink)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.InsertAsync(mediaLink);
    }

    public async Task<int> UpdateMediaLinkAsync(MediaLink mediaLink)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.UpdateAsync(mediaLink);
    }

    public async Task<int> DeleteMediaLinkAsync(MediaLink mediaLink)
    {
        if (!isInitialized)
            await InitializeAsync();
        return await Database.DeleteAsync(mediaLink);
    }
}