namespace SmartGenealogy.ViewModels;

public partial class MainViewModel : BaseViewModel, IRecipient<CultureChangeMessage>
{
    [ObservableProperty]
    private bool isRTLLanguage;

    public MainViewModel()
    {
        WeakReferenceMessenger.Default.Register<CultureChangeMessage>(this);
        IsRTLLanguage = AppSettings.IsRTLLanguage;
    }

    /// <summary>
    /// On received culture changed message, put your action inside MainThread
    /// </summary>
    /// <param name="message"></param>
    public void Receive(CultureChangeMessage message)
    {
        IsRTLLanguage = AppSettings.IsRTLLanguage;
        MainThread.BeginInvokeOnMainThread(() =>
        {
        });
    }

    [RelayCommand]
    private async Task CreateFileTapped()
    {
        var popupViewModel = new CreateFilePopupViewModel();
        var popup = new CreateFilePopupPage { BindingContext = popupViewModel };
        await PopupAction.DisplayPopup(popup);

        // Await the result from the popup
        var result = await popupViewModel.PopupClosedTask;
        if (result != null)
        {
            //await Shell.Current.GoToAsync(nameof(AISettingsPage));
        }
    }

    [RelayCommand]
    private async Task OpenFileTapped()
    {
        var customFileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { ".sgdb" } },
                { DevicePlatform.iOS, new[] { ".sgdb" } },
                { DevicePlatform.WinUI, new[] { ".sgdb" } },
                { DevicePlatform.MacCatalyst, new[] { "sgdb" } },
            });

        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select a database",
            //FileTypes = customFileType,
        });
        if (result != null)
        {
            if (!string.IsNullOrEmpty(result!.FileName))
            {
                //FilePath = result.Folder.Path;
            }
        }
    }

    public async Task OnNavigatedToAsync()
    {
        await Task.CompletedTask;
    }

    public async Task OnNavigatedFromAsync()
    {
        await Task.CompletedTask;
    }
}