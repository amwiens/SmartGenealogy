using System.Text.Json;

namespace SmartGenealogy.Data;

public class DatabaseInitializer
{
    private readonly PersonRepository _personRepository;
    private readonly FactTypeRepository _factTypeRepository;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(PersonRepository personRepository, FactTypeRepository factTypeRepository)
    {
        _personRepository = personRepository;
        _factTypeRepository = factTypeRepository;
    }

    public async Task LoadSeedDataAsync()
    {
        await using Stream templateStream = await FileSystem.OpenAppPackageFileAsync("FactTypes.json");

        FactTypesJson? payload = null;
        try
        {
            payload = JsonSerializer.Deserialize(templateStream, JsonContext.Default.FactTypesJson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deserializing seed data from JSON.");
        }

        try
        {
            if (payload is not null)
            {
                foreach (var factType in payload.FactTypes)
                {
                    await _factTypeRepository.SaveItemAsync(factType);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving seed data");
            throw;
        }
    }
            
    private async void ClearTables()
    {
        try
        {
            await Task.WhenAll(
                _factTypeRepository.DropTableAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing database tables during seed data reset.");
            throw;
        }
    }
}