namespace SmartGenealogy.Core.ViewModels.Tools;

/// <summary>
/// Web links page view model
/// </summary>
/// <param name="webLinkRepository">Fact Type repository</param>
/// <param name="popupService">Popup service</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class WebLinksPageViewModel(
    WebLinkRepository webLinkRepository,
    IPopupService popupService,
    ModalErrorHandler errorHandler) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<WebLink> _webLinks = [];

    [ObservableProperty]
    private WebLink? _selectedItem;

    /// <summary>
    /// Appearing
    /// </summary>
    [RelayCommand]
    private async Task Appearing()
    {
        WebLinks = new ObservableCollection<WebLink>(await webLinkRepository.ListAsync());
    }

    /// <summary>
    /// Add web link.
    /// </summary>
    [RelayCommand]
    private async Task AddWebLink()
    {
        try
        {
            if (Shell.Current is Shell shell)
            {
                var result = await popupService.ShowPopupAsync<AddEditFactTypePopupViewModel>(shell);
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Open Fact type details
    /// </summary>
    [RelayCommand]
    private async Task OpenWebLinkDetails()
    {
        if (SelectedItem is not null)
            await Shell.Current.GoToAsync($"factTypeDetails?id={SelectedItem.Id}");
    }
}