namespace SmartGenealogy.Core.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly FactTypeRepository _factTypeRepository;
    private readonly DatabaseSettings _databaseSettings;
    private readonly ModalErrorHandler _modalErrorHandler;

    public MainPageViewModel(
        FactTypeRepository factTypeRepository,
        DatabaseSettings databaseSettings,
        ModalErrorHandler modalErrorHandler)
    {
        _factTypeRepository = factTypeRepository;
        _databaseSettings = databaseSettings;
        _modalErrorHandler = modalErrorHandler;
    }

    [RelayCommand]
    private async Task CreateDatabase()
    {
        try
        {
            _databaseSettings.DatabaseFilename = "genealogy.db";
            _databaseSettings.DatabasePath = @"C:\Code";

            await _factTypeRepository.CreateTableAsync();
        }
        catch (Exception ex)
        {
            _modalErrorHandler.HandleError(ex);
        }
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