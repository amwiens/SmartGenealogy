namespace SmartGenealogy.Core.ViewModels.Popups;

/// <summary>
/// New database popup view model.
/// </summary>
/// <param name="databaseSettings">Database settings.</param>
/// <param name="popupService">Popup service</param>
public partial class NewDatabasePopupViewModel(DatabaseSettings databaseSettings, IPopupService popupService)
    : ObservableObject
{
    [ObservableProperty]
    private string _databaseName = string.Empty;

    [ObservableProperty]
    private string _databasePath = string.Empty;

    bool CanCreate() => string.IsNullOrWhiteSpace(DatabaseName) is false && string.IsNullOrWhiteSpace(DatabasePath) is false;

    /// <summary>
    /// Select folder.
    /// </summary>
    [RelayCommand]
    private async Task SelectFolder()
    {

    }

    /// <summary>
    /// Create database.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanCreate))]
    private async Task Create()
    {
        databaseSettings.DatabaseFilename = $"{DatabaseName}.sgdb";
        databaseSettings.DatabasePath = DatabasePath;

        await popupService.ClosePopupAsync(Shell.Current);
    }

    /// <summary>
    /// Cancel
    /// </summary>
    [RelayCommand]
    private async Task Cancel()
    {
        await popupService.ClosePopupAsync(Shell.Current);
    }
}