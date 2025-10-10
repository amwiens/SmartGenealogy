namespace SmartGenealogy.Data.Services;

public class SeedDataService(
    FactTypeRepository factTypeRepository,
    MultimediaLineRepository multimediaLineRepository,
    MultimediaRepository multimediaRepository,
    MultimediaWordRepository multimediaWordRepository,
    PlaceRepository placeRepository,
    RoleRepository roleRepository,
    ILogger<SeedDataService> logger)
{
    private readonly string _seedDataFilePath = "SeedData.json";

    /// <summary>
    /// Creates the database.
    /// </summary>
    public async Task LoadSeedDataAsync()
    {
        CreateTables();

        await using Stream templateStream = await FileSystem.OpenAppPackageFileAsync(_seedDataFilePath);

        FactTypeJson? payload = null;
        try
        {
            payload = JsonSerializer.Deserialize(templateStream, JsonContext.Default.FactTypeJson);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deserializing seed data");
        }

        try
        {
            if (payload is not null)
            {
                foreach (var factType in payload.FactTypes)
                {
                    if (factType is null)
                    {
                        continue;
                    }

                    await factTypeRepository.SaveItemAsync(factType);

                    if (factType?.Roles is not null)
                    {
                        foreach (var role in factType.Roles)
                        {
                            role.EventType = factType.Id;
                            await roleRepository.SaveItemAsync(role);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving seed data");
            throw;
        }
    }

    /// <summary>
    /// Update the database.
    /// </summary>
    public async Task UpdateDatabaseAsync()
    {
        CreateTables();
    }

    /// <summary>
    /// Create the tables in the database.
    /// </summary>
    private async void CreateTables()
    {
        try
        {
            await Task.WhenAll(
                factTypeRepository.CreateTableAsync(),
                multimediaLineRepository.CreateTableAsync(),
                multimediaRepository.CreateTableAsync(),
                multimediaWordRepository.CreateTableAsync(),
                placeRepository.CreateTableAsync(),
                roleRepository.CreateTableAsync()
                );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating database tables during seed data reset");
        }
    }
}