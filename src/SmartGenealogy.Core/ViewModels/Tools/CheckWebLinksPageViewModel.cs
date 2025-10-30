namespace SmartGenealogy.Core.ViewModels.Tools;

/// <summary>
/// Check web links page view model
/// </summary>
/// <param name="webLinkRepository">Fact Type repository</param>
/// <param name="popupService">Popup service</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class CheckWebLinksPageViewModel(
    WebLinkRepository webLinkRepository,
    IPopupService popupService,
    ModalErrorHandler errorHandler) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<WebLink> _webLinks = [];

    [ObservableProperty]
    private WebLink? _selectedItem;

    [ObservableProperty]
    private bool _isBusy = false;

    /// <summary>
    /// Appearing
    /// </summary>
    [RelayCommand]
    private async Task Appearing()
    {
        var webLinks = await webLinkRepository.ListAsync();
        WebLinks = new ObservableCollection<WebLink>(webLinks.OrderBy(x => x.Name));
    }

    /// <summary>
    /// Add web link.
    /// </summary>
    [RelayCommand]
    private async Task CheckWebLinks()
    {
        List<WebLink> brokenWebLinks = [];

        try
        {
            IsBusy = true;
            var webLinks = await webLinkRepository.ListAsync();
            using var httpClient = new HttpClient();

            foreach (var webLink in webLinks)
            {
                if (!string.IsNullOrEmpty(webLink.URL))
                {
                    try
                    {
                        var response = await httpClient.GetAsync(webLink.URL);
                        if (!response.IsSuccessStatusCode)
                        {
                            brokenWebLinks.Add(webLink);
                        }
                    }
                    catch
                    {
                        // If an exception occurs (e.g., network error), consider the link broken
                        brokenWebLinks.Add(webLink);
                    }
                }
            }

            WebLinks = new ObservableCollection<WebLink>(brokenWebLinks.OrderBy(x => x.Name));
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Open Fact type details
    /// </summary>
    [RelayCommand]
    private async Task OpenWebLinkDetails()
    {
        if (SelectedItem is not null)
            await Shell.Current.GoToAsync($"webLink?id={SelectedItem.Id}");
    }
}