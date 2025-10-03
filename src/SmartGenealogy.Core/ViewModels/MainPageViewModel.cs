namespace SmartGenealogy.Core.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly FactTypeRepository _factTypeRepository;
    private readonly DatabaseSettings _databaseSettings;

    public MainPageViewModel(
        FactTypeRepository factTypeRepository,
        DatabaseSettings databaseSettings)
    {
        _factTypeRepository = factTypeRepository;
        _databaseSettings = databaseSettings;
    }

    [RelayCommand]
    private async Task CreateDatabase()
    {
        _databaseSettings.DatabaseFilename = "genealogy.db";
        _databaseSettings.DatabasePath = @"C:\Code";

        await _factTypeRepository.CreateTableAsync();
    }

    [RelayCommand]
    private async Task OpenDatabase()
    {

    }

    [RelayCommand]
    private async Task CloseDatabase()
    {

    }
}