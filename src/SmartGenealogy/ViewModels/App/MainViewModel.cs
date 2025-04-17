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
    private async void CreateFileTapped()
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

    public async Task OnNavigatedToAsync()
    {
        await Task.CompletedTask;
    }

    public async Task OnNavigatedFromAsync()
    {
        await Task.CompletedTask;
    }
}