namespace SmartGenealogy.ViewModels.Main;

public partial class MainPageViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DatabaseSettings _databaseSettings;
    private readonly IPopupService _popupService;

    [ObservableProperty]
    private string? _title = "Main Page";

    [ObservableProperty]
    private bool _isDatabaseOpen = false;

    public MainPageViewModel(IServiceProvider serviceProvider, DatabaseSettings databaseSettings, IPopupService popupService)
    {
        _serviceProvider = serviceProvider;
        _databaseSettings = databaseSettings;
        _popupService = popupService;
    }

    [RelayCommand]
    private async Task CreateDatabase()
    {
        var popupResult = await _popupService.ShowPopupAsync<NewDatabasePopupPage>(Application.Current!.Windows[0].Page!);
    }

    [RelayCommand]
    private async Task OpenDatabase()
    {
        try
        {
            var options = new PickOptions
            {
                PickerTitle = "Please select a database file",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.database" } }, // UTType values
                    { DevicePlatform.Android, new[] { "application/*" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { ".sgdb", ".db", ".sqlite", ".sqlite3" } }, // file extensions
                    { DevicePlatform.Tizen, new[] { "*/*" } }, // MIME type
                    { DevicePlatform.macOS, new[] { "public.database" } }, // UTType values
                })
            };

            var result = await FilePicker.Default.PickAsync(options);

            if (result != null)
            {
                // Process the selected file
                if (File.Exists(result.FullPath))
                {
                    var fi = new FileInfo(result.FullPath);
                    _databaseSettings.DatabasePath = fi.DirectoryName;
                    _databaseSettings.DatabaseName = fi.Name;

                    IsDatabaseOpen = true;
                    WeakReferenceMessenger.Default.Send(new DatabaseOpenMessage(result.FullPath));
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    [RelayCommand]
    private void Appearing()
    {
        if (_databaseSettings.DatabasePath is not null && !string.IsNullOrEmpty(_databaseSettings.DatabasePath))
            IsDatabaseOpen = true;
    }

    [RelayCommand]
    private void CloseDatabase()
    {
        _databaseSettings.DatabasePath = string.Empty;
        _databaseSettings.DatabaseName = string.Empty;
        IsDatabaseOpen = false;
        WeakReferenceMessenger.Default.Send(new DatabaseOpenMessage(string.Empty));
    }
}