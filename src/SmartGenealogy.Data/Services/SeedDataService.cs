namespace SmartGenealogy.Data.Services;

/// <summary>
/// Seed data service.
/// </summary>
/// <param name="factTypeRepository">Fact type repository</param>
/// <param name="mediaLinkRepository">Media link repository</param>
/// <param name="multimediaLineRepository">Multimedia line repository</param>
/// <param name="multimediaRepository">Multimedia repository</param>
/// <param name="multimediaWordRepository">Multimedia word repository</param>
/// <param name="placeRepository">Place repository</param>
/// <param name="projectRepository">Project repository</param>
/// <param name="roleRepository">Role repository</param>
/// <param name="sourceRepository">Source repository</param>
/// <param name="webLinkLinkRepository">Weblink link repository</param>
/// <param name="webLinkRepository">Web link repository</param>
/// <param name="logger">Logger</param>
public class SeedDataService(
    FactTypeRepository factTypeRepository,
    MediaLinkRepository mediaLinkRepository,
    MultimediaLineRepository multimediaLineRepository,
    MultimediaRepository multimediaRepository,
    MultimediaWordRepository multimediaWordRepository,
    PlaceRepository placeRepository,
    ProjectRepository projectRepository,
    RoleRepository roleRepository,
    SourceRepository sourceRepository,
    WebLinkLinkRepository webLinkLinkRepository,
    WebLinkRepository webLinkRepository,
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
                mediaLinkRepository.CreateTableAsync(),
                multimediaLineRepository.CreateTableAsync(),
                multimediaRepository.CreateTableAsync(),
                multimediaWordRepository.CreateTableAsync(),
                placeRepository.CreateTableAsync(),
                projectRepository.CreateTableAsync(),
                roleRepository.CreateTableAsync(),
                sourceRepository.CreateTableAsync(),
                webLinkLinkRepository.CreateTableAsync(),
                webLinkRepository.CreateTableAsync()
                );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating database tables during seed data reset");
        }
    }
}