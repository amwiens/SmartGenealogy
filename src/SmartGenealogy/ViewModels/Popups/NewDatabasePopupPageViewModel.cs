namespace SmartGenealogy.ViewModels.Popups;

public partial class NewDatabasePopupPageViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DatabaseSettings _databaseSettings;
    private readonly IPopupService _popupService;

    [ObservableProperty]
    private string? _databaseName;

    [ObservableProperty]
    private string? _databasePath;

    bool CanSubmitButtonExeute => DatabaseName is not null && DatabaseName!.Length > 0 && DatabasePath is not null && DatabasePath!.Length > 0;

    public NewDatabasePopupPageViewModel(IServiceProvider serviceProvider, DatabaseSettings databaseSettings, IPopupService popupService)
    {
        _serviceProvider = serviceProvider;
        _databaseSettings = databaseSettings;
        _popupService = popupService;
    }

    [RelayCommand]
    private async Task SelectFolder()
    {
        try
        {
            var result = await FolderPicker.Default.PickAsync();
            if (result != null && result.Folder != null)
            {
                DatabasePath = result.Folder!.Path;
            }
        }
        catch { }
    }


    [RelayCommand(CanExecute = nameof(CanSubmitButtonExeute))]
    private async Task Submit()
    {
        var name = $"{DatabaseName!.Trim()}.sgdb";
        var path = DatabasePath!.Trim();

        _databaseSettings.DatabaseName = name;
        _databaseSettings.DatabasePath = path;

        var databaseInitializer = _serviceProvider.GetRequiredService<DatabaseInitializer>();
        await databaseInitializer.LoadSeedDataAsync();

        WeakReferenceMessenger.Default.Send(new DatabaseOpenMessage(Path.Combine(path, name)));
        await _popupService.ClosePopupAsync(Application.Current!.Windows[0].Page!);
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await _popupService.ClosePopupAsync(Application.Current!.Windows[0].Page!);
    }
}