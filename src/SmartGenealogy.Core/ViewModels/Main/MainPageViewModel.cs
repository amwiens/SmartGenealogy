namespace SmartGenealogy.Core.ViewModels.Main;

/// <summary>
/// Main Page View Model.
/// </summary>
/// <param name="seedDataService">Seed data service.</param>
/// <param name="databaseSettings">Database settings.</param>
/// <param name="modalErrorHandler">Modal error handler.</param>
public partial class MainPageViewModel(
    SeedDataService seedDataService,
    DatabaseSettings databaseSettings,
    ModalErrorHandler modalErrorHandler)
    : ObservableObject
{

    /// <summary>
    /// Create a new database.
    /// </summary>
    [RelayCommand]
    private async Task CreateDatabase()
    {
        try
        {
            databaseSettings.DatabaseFilename = "genealogy.db";
            databaseSettings.DatabasePath = @"C:\Code";

            await seedDataService.LoadSeedDataAsync();
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

    }
}