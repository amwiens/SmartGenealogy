using CommunityToolkit.Maui.Storage;

namespace SmartGenealogy.ViewModels;

public partial class NewDatabasePopupViewModel : ObservableObject
{
    private TaskCompletionSource<NewDatabaseItem> _taskCompletionSource;
    public Task<NewDatabaseItem> PopupClosedTask => _taskCompletionSource.Task;

    public NewDatabasePopupViewModel()
    {
        NewDatabase = new NewDatabaseItem();
        _taskCompletionSource = new TaskCompletionSource<NewDatabaseItem>();
    }

    [ObservableProperty]
    private NewDatabaseItem _newDatabase;

    [ObservableProperty]
    private string? _databaseName = string.Empty;

    [ObservableProperty]
    private string? _databasePath = string.Empty;

    [RelayCommand]
    private async Task OkTapped()
    {
        NewDatabase.DatabaseName = DatabaseName;
        NewDatabase.DatabasePath = DatabasePath;
        // Set the result and close the popup
        _taskCompletionSource.SetResult(NewDatabase);

        await PopupAction.ClosePopup();
    }

    [RelayCommand]
    private async Task CancelTapped()
    {
        await PopupAction.ClosePopup();
    }

    [RelayCommand]
    private async Task SelectFolder()
    {
        var cancellationToken = new CancellationToken();
        var folder = await FolderPicker.PickAsync(cancellationToken);

        DatabasePath = folder.Folder!.Path;
    }
}