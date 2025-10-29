namespace SmartGenealogy.Core.ViewModels.Popups;

/// <summary>
/// Select web link popup view model.
/// </summary>
/// <param name="webLinkRepository">Web link repository</param>
/// <param name="popupService">Popup service</param>
public partial class SelectWebLinkPopupViewModel(
    WebLinkRepository webLinkRepository,
    IPopupService popupService)
    : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Data.Models.WebLink> _webLink = [];

    [ObservableProperty]
    private WebLink? _selectedItem;

    /// <summary>
    /// Appearing
    /// </summary>
    [RelayCommand]
    private async Task Appearing()
    {
        var multimedia = await webLinkRepository.ListAsync();
        WebLink = new ObservableCollection<WebLink>(multimedia.OrderBy(x => x.Name));
    }

    /// <summary>
    /// Save
    /// </summary>
    [RelayCommand]
    private async Task Save()
    {
        if (SelectedItem is null)
            return;

        await popupService.ClosePopupAsync(Shell.Current, SelectedItem.Id);
    }

    /// <summary>
    /// Cancel
    /// </summary>
    [RelayCommand]
    private async Task Cancel()
    {
        await popupService.ClosePopupAsync(Shell.Current);
    }
}