namespace SmartGenealogy.Core.ViewModels.Main;

/// <summary>
/// Main Page View Model.
/// </summary>
/// <param name="seedDataService">Seed data service.</param>
/// <param name="databaseSettings">Database settings.</param>
/// <param name="popupService">Popup service.</param>
/// <param name="modalErrorHandler">Modal error handler.</param>
public partial class MainPageViewModel(
    SeedDataService seedDataService,
    DatabaseSettings databaseSettings,
    IPopupService popupService,
    ModalErrorHandler modalErrorHandler)
    : ObservableObject
{
    [ObservableProperty]
    private bool _databaseOpen = false;

    /// <summary>
    /// Create a new database.
    /// </summary>
    [RelayCommand]
    private async Task CreateDatabase()
    {
        try
        {
            var result = await popupService.ShowPopupAsync<NewDatabasePopupViewModel>(Shell.Current);

            if (!string.IsNullOrWhiteSpace(databaseSettings.DatabaseFilename) && !string.IsNullOrWhiteSpace(databaseSettings.DatabasePath))
            {
                await seedDataService.LoadSeedDataAsync();
                DatabaseOpen = true;
            }
        }
        catch (Exception ex)
        {
            modalErrorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Open an existing database.
    /// </summary>
    [RelayCommand]
    private async Task OpenDatabase()
    {

    }

    /// <summary>
    /// Close the open database.
    /// </summary>
    [RelayCommand]
    private async Task CloseDatabase()
    {
        databaseSettings.DatabaseFilename = null;
        databaseSettings.DatabasePath = null;
        DatabaseOpen = false;
    }
}