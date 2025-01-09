namespace SmartGenealogy.ViewModels;

public partial class LanguageSelectionPopupViewModel : ObservableObject
{
    private TaskCompletionSource<LanguageSelectItem> _taskCompletionSource;

    public Task<LanguageSelectItem> PopupClosedTask => _taskCompletionSource.Task;

    public LanguageSelectionPopupViewModel()
    {
        LanguageLists = AppSettings.Languages;
        _taskCompletionSource = new TaskCompletionSource<LanguageSelectItem>();
    }

    [ObservableProperty]
    private List<LanguageSelectItem> _languageLists;

    [ObservableProperty]
    private LanguageSelectItem _languageSelected;

    [RelayCommand]
    private async Task OkTapped()
    {
        // Set the result and close the popup
        _taskCompletionSource.SetResult(LanguageSelected);

        await PopupAction.ClosePopup(LanguageSelected.Code);
    }

    [RelayCommand]
    private async Task CancelTapped()
    {
        await PopupAction.ClosePopup();
    }
}