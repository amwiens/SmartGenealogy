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
    [NotifyCanExecuteChangedFor(nameof(CreateCommand))]
    private string _databaseName = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateCommand))]
    private string _databasePath = string.Empty;

    bool CanCreate() => string.IsNullOrWhiteSpace(DatabaseName) is false && string.IsNullOrWhiteSpace(DatabasePath) is false;

    /// <summary>
    /// Select folder.
    /// </summary>
    [RelayCommand]
    private async Task SelectFolder()
    {
        var result = await FolderPicker.Default.PickAsync();

        if (result.IsSuccessful)
        {
            DatabasePath = result.Folder.Path;
        }
    }

    /// <summary>
    /// Create database.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanCreate))]
    private async Task Create()
    {
        var databaseFileName = $"{DatabaseName}.sgdb";
        var databasePath = Path.Combine(DatabasePath, databaseFileName);

        if (File.Exists(databasePath))
        {
            if (Shell.Current is Shell shell)
                await shell.DisplayAlertAsync("Error", "Database already exists.", "OK");
        }
        else
        {
            databaseSettings.DatabaseFilename = databaseFileName;
            databaseSettings.DatabasePath = DatabasePath;

            await popupService.ClosePopupAsync(Shell.Current);
        }
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