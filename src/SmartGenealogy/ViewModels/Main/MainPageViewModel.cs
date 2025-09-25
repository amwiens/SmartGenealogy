using SmartGenealogy.Data.Models;
using SmartGenealogy.Data.Repositories;
using SmartGenealogy.Data.Settings;

namespace SmartGenealogy.ViewModels.Main;

public partial class MainPageViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DatabaseSettings _databaseSettings;

    [ObservableProperty]
    private string? _title = "Main Page";

    [ObservableProperty]
    private bool _isDatabaseOpen = false;

    public MainPageViewModel(IServiceProvider serviceProvider, DatabaseSettings databaseSettings)
    {
        _serviceProvider = serviceProvider;
        _databaseSettings = databaseSettings;

    }

    [RelayCommand]
    private async Task CreateDatabase()
    {
        await PopupNavigation.Instance.PushAsync(new NewDatabasePopupPage(_serviceProvider, _databaseSettings));
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
    private async Task Appearing()
    {
        if (_databaseSettings.DatabasePath is not null && !string.IsNullOrEmpty(_databaseSettings.DatabasePath))
            IsDatabaseOpen = true;
    }

    [RelayCommand]
    private async Task CloseDatabase()
    {
        _databaseSettings.DatabasePath = string.Empty;
        _databaseSettings.DatabaseName = string.Empty;
        IsDatabaseOpen = false;
        WeakReferenceMessenger.Default.Send(new DatabaseOpenMessage(string.Empty));
    }
}