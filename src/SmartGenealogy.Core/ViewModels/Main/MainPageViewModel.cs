namespace SmartGenealogy.Core.ViewModels.Main;

/// <summary>
/// Main Page View Model.
/// </summary>
public partial class MainPageViewModel : ObservableObject, IRecipient<OpenDatabaseMessage>
{
    private readonly SeedDataService _seedDataService;
    private readonly DatabaseSettings _databaseSettings;
    private readonly IPopupService _popupService;
    private readonly ModalErrorHandler _errorHandler;

    [ObservableProperty]
    private bool _databaseOpen = false;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="seedDataService">Seed data service.</param>
    /// <param name="databaseSettings">Database settings.</param>
    /// <param name="popupService">Popup service.</param>
    /// <param name="errorHandler">Modal error handler.</param>
    public MainPageViewModel(
        SeedDataService seedDataService,
        DatabaseSettings databaseSettings,
        IPopupService popupService,
        ModalErrorHandler errorHandler)
    {
        _seedDataService = seedDataService;
        _databaseSettings = databaseSettings;
        _popupService = popupService;
        _errorHandler = errorHandler;

        WeakReferenceMessenger.Default.Register<OpenDatabaseMessage>(this);
    }

    /// <summary>
    /// Appearing.
    /// </summary>
    [RelayCommand]
    private void Appearing()
    {
    }

    /// <summary>
    /// Receives Open Database messages
    /// </summary>
    /// <param name="message">Open database message</param>
    public void Receive(OpenDatabaseMessage message)
    {
        if (_databaseSettings.DatabasePath is not null && !string.IsNullOrEmpty(_databaseSettings.DatabasePath))
            DatabaseOpen = true;
    }

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
                var result = await _popupService.ShowPopupAsync<NewDatabasePopupViewModel>(shell);

                if (!string.IsNullOrWhiteSpace(_databaseSettings.DatabaseFilename) && !string.IsNullOrWhiteSpace(_databaseSettings.DatabasePath))
                {
                    await _seedDataService.LoadSeedDataAsync();
                    WeakReferenceMessenger.Default.Send(new OpenDatabaseMessage(true));
                    DatabaseOpen = true;
                    SmartGenealogySettings.LastOpenDatabase = Path.Combine(_databaseSettings.DatabasePath, _databaseSettings.DatabaseFilename);
                    SmartGenealogySettings.SaveSettings();
                }
            }
        }
        catch (Exception ex)
        {
            _errorHandler.HandleError(ex);
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

                    _databaseSettings.DatabaseFilename = fileInfo.Name;
                    _databaseSettings.DatabasePath = fileInfo.Directory!.FullName;
                    WeakReferenceMessenger.Default.Send(new OpenDatabaseMessage(true));
                    DatabaseOpen = true;
                    SmartGenealogySettings.LastOpenDatabase = Path.Combine(_databaseSettings.DatabasePath, _databaseSettings.DatabaseFilename);
                    SmartGenealogySettings.SaveSettings();
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
            _errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Close the open database.
    /// </summary>
    [RelayCommand]
    private void CloseDatabase()
    {
        _databaseSettings.DatabaseFilename = null;
        _databaseSettings.DatabasePath = null;
        WeakReferenceMessenger.Default.Send(new OpenDatabaseMessage(false));
        DatabaseOpen = false;
        SmartGenealogySettings.LastOpenDatabase = string.Empty;
        SmartGenealogySettings.SaveSettings();
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

        for (int i = 0; i < sQLiteHeader.Length; i++)
        {
            if (sQLiteHeader[i] != fileHeader[i])
                return false;
        }

        return true;
    }
}