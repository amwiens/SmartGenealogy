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
            if (Shell.Current is Shell shell)
            {
                var result = await popupService.ShowPopupAsync<NewDatabasePopupViewModel>(shell);

                if (!string.IsNullOrWhiteSpace(databaseSettings.DatabaseFilename) && !string.IsNullOrWhiteSpace(databaseSettings.DatabasePath))
                {
                    await seedDataService.LoadSeedDataAsync();
                    DatabaseOpen = true;
                }
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
        try
        {
            var databaseFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".sgdb" } }
                });

            var options = new PickOptions
            {
                PickerTitle = "Please select a file",
                FileTypes = databaseFileType
            };

            var result = await FilePicker.Default.PickAsync(options);

            if (result != null)
            {
                if (File.Exists(result.FullPath) && IsSQLiteDatabase(result.FullPath))
                {
                    var fileInfo = new FileInfo(result.FullPath);

                    databaseSettings.DatabaseFilename = fileInfo.Name;
                    databaseSettings.DatabasePath = fileInfo.Directory!.FullName;
                    DatabaseOpen = true;
                }
                else
                {
                    if (Shell.Current is Shell shell)
                        await shell.DisplayAlertAsync("Error", "Unable to open database file.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            modalErrorHandler.HandleError(ex);
        }
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

    private bool IsSQLiteDatabase(string fileName)
    {
        var sQLiteHeader = System.Text.Encoding.ASCII.GetBytes("SQLite format 3\0");
        if (!File.Exists(fileName))
            throw new FileNotFoundException("The specified file does not exist.");

        var fileHeader = new byte[16];
        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            if (fs.Length < 16)
                return false;

            fs.ReadExactly(fileHeader, 0, 16);
        }

        // Compare Headers
        if (sQLiteHeader.Length != fileHeader.Length)
            return false;

        for (int i = 0; i <  sQLiteHeader.Length; i++)
        {
            if (sQLiteHeader[i] != fileHeader[i])
                return false;
        }

        return true;
    }
}