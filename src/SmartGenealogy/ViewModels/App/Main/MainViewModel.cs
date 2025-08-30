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
    private async Task CreateFile()
    {

    }

    [RelayCommand]
    private async Task OpenFile()
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select a file to open",
            });
            if (result != null)
            {
                // You can access the file with result.FullPath or result.FileName
                // Example: read file content
                using var stream = await result.OpenReadAsync();
                // TODO: Process the file stream as needed
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions, e.g., user canceled or file access error
            // Optionally set error state or show a message
        }
    }
}