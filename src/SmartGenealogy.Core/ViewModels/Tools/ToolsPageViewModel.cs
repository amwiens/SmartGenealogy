
namespace SmartGenealogy.Core.ViewModels.Tools;

/// <summary>
/// Tools page view model.
/// </summary>
/// <param name="databaseTools">Database tools</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class ToolsPageViewModel(
    DatabaseTools databaseTools,
    ModalErrorHandler errorHandler) : ObservableObject
{
    /// <summary>
    /// Compact database.
    /// </summary>
    [RelayCommand]
    private async Task CompactDatabase()
    {
        try
        {
            await databaseTools.CompactDatabase();
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
        await Toast.Make("Database compact finished").Show();
    }

    /// <summary>
    /// Reindex database.
    /// </summary>
    [RelayCommand]
    private async Task ReindexDatabase()
    {
        try
        {
            await databaseTools.ReindexDatabase();
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
        await Toast.Make("Re-indexing finished").Show();
    }

    /// <summary>
    /// Database integrity check.
    /// </summary>
    [RelayCommand]
    private async Task DatabaseIntegrityCheck()
    {
        try
        {
            await databaseTools.DatabaseIntegrityCheck();
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
        await Toast.Make("Integrity check finished").Show();
    }

    /// <summary>
    /// Open Fact Types page.
    /// </summary>
    [RelayCommand]
    private async Task OpenFactTypesPage()
    {
        await Shell.Current.GoToAsync("factTypes");
    }
}