namespace SmartGenealogy.Core.ViewModels.Tools;

/// <summary>
/// Web link page view model.
/// </summary>
/// <param name="webLinkRepository">Web link repository</param>
/// <param name="alertService">Alert service</param>
/// <param name="popupService">Popup service</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class WebLinkPageViewModel(
    WebLinkRepository webLinkRepository,
    IAlertService alertService,
    IPopupService popupService,
    ModalErrorHandler errorHandler)
    : ObservableObject, IQueryAttributable
{
    private WebLink? _webLink;

    [ObservableProperty]
    private string? _name = string.Empty;

    [ObservableProperty]
    private string? _url = string.Empty;

    [ObservableProperty]
    private string? _note = string.Empty;

    /// <summary>
    /// Apply attributes.
    /// </summary>
    /// <param name="query">Query</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("id"))
        {
            int webLinkId = Convert.ToInt32(query["id"]);
            LoadData(webLinkId).FireAndForgetSafeAsync();
        }
    }

    /// <summary>
    /// Load data.
    /// </summary>
    /// <param name="id">Record identifier</param>
    /// <returns></returns>
    private async Task LoadData(int id)
    {
        try
        {
            _webLink = await webLinkRepository.GetAsync(id);

            if (_webLink.IsNullOrNew())
            {
                errorHandler.HandleError(new Exception($"Fact type with id {id} could not be found."));
                return;
            }

            Name = _webLink.Name;
            Url = _webLink.URL;
            Note = _webLink.Note;
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Edit fact type.
    /// </summary>
    [RelayCommand]
    private async Task EditFactType()
    {
        var queryAttributes = new Dictionary<string, object>
        {
            { "id", _webLink!.Id }
        };

        await popupService.ShowPopupAsync<AddEditFactTypePopupViewModel>(
            Shell.Current,
            options: PopupOptions.Empty,
            shellParameters: queryAttributes);
    }

    /// <summary>
    /// Delete fact type.
    /// </summary>
    [RelayCommand]
    private async Task DeleteFactType()
    {
        try
        {
            var isConfirmed = await alertService.ShowAlertAsync("Delete fact type", "Are you sure you want to delete this fact type?", "Yes", "No");
            if (isConfirmed)
            {
                //var isDeleted = await factTypeService.DeleteItemAsync(_factType!);
                //if (isDeleted)
                //    await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }
}