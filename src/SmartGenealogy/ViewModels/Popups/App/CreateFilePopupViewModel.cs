using CommunityToolkit.Maui.Storage;

namespace SmartGenealogy.ViewModels.Popups.App;

public partial class CreateFilePopupViewModel : BaseViewModel
{
    private TaskCompletionSource<string> _taskCompletionSource;
    public Task<string> PopupClosedTask => _taskCompletionSource.Task;

    [ObservableProperty]
    private string? filePath;

    [ObservableProperty]
    private string? fileName;

    public CreateFilePopupViewModel()
    {
        _taskCompletionSource = new TaskCompletionSource<string>();
    }



    [RelayCommand]
    private async Task PickFolderPath()
    {
        var result = await FolderPicker.Default.PickAsync();
        if (result.IsSuccessful)
        {
            FilePath = result.Folder.Path;
        }
    }

    [RelayCommand]
    private async Task OkTapped()
    {
        // Set the result and close the popup
        if (string.IsNullOrEmpty(FilePath) || string.IsNullOrEmpty(FileName))
        {

            await Application.Current!.Windows[0].Page!.DisplayAlert("Error", "Please select a folder and enter a file name.", AppTranslations.ButtonOk);
            return;
        }
        if (!Directory.Exists(FilePath))
        {
            await Application.Current!.Windows[0].Page!.DisplayAlert("Error", "The selected folder does not exist.", AppTranslations.ButtonOk);
            return;
        }
        var file = Path.Combine(FilePath!, FileName!, ".sgdb");
        if (File.Exists(file))
        {
            await Application.Current!.Windows[0].Page!.DisplayAlert("Error", "The file already exists.", AppTranslations.ButtonOk);
            return;
        }
        _taskCompletionSource.SetResult(file);
        await PopupAction.ClosePopup(file);
    }

    [RelayCommand]
    private async Task CancelTapped()
    {
        await PopupAction.ClosePopup();
    }
}