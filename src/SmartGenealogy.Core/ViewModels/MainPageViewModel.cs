namespace SmartGenealogy.Core.ViewModels;

/// <summary>
/// Main Page View Model.
/// </summary>
/// <param name="factTypeRepository">Fact type repository.</param>
/// <param name="databaseSettings">Database settings.</param>
/// <param name="modalErrorHandler">Modal error handler.</param>
public partial class MainPageViewModel(
    FactTypeRepository factTypeRepository,
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

            await factTypeRepository.CreateTableAsync();
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