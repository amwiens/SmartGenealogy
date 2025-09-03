using SmartGenealogy.Data;
using SmartGenealogy.Data.Models;

namespace SmartGenealogy.ViewModels;

public partial class MainViewModel : BaseViewModel, IRecipient<CultureChangeMessage>
{
    private readonly PersonRepository _personRepository;
    private readonly FactTypeRepository _factTypeRepository;
    private readonly DatabaseSettings _databaseSettings;

    [ObservableProperty]
    private bool isRTLLanguage;

    public MainViewModel(PersonRepository personRepository, FactTypeRepository factTypeRepository, DatabaseSettings databaseSettings)
    {
        _personRepository = personRepository;
        _factTypeRepository = factTypeRepository;
        _databaseSettings = databaseSettings;

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
        var dbPath = Path.Combine(@"C:\Code\Mine", "smartgenealogy.db");
        _databaseSettings.DatabasePath = dbPath;
        _databaseSettings.DatabaseName = "smartgenealogy.db";

        var database = new DatabaseInitializer(_personRepository, _factTypeRepository);
        await database.LoadSeedDataAsync();
        WeakReferenceMessenger.Default.Send(new DatabaseChangeMessage(true));
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
                _databaseSettings.DatabasePath = result.FullPath;
                _databaseSettings.DatabaseName = result.FileName;

                await _personRepository.SaveItemAsync(new Person { Sex = 0, ParentID = 1, DateCreated = DateTime.UtcNow, DateUpdated = DateTime.UtcNow });
                // You can access the file with result.FullPath or result.FileName
                using var stream = await result.OpenReadAsync();
                // TODO: Process the file stream as needed
                WeakReferenceMessenger.Default.Send(new DatabaseChangeMessage(true));
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions, e.g., user canceled or file access error
            // Optionally set error state or show a message
        }
    }
}