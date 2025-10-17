﻿namespace SmartGenealogy.Core.ViewModels.Popups;

/// <summary>
/// Add edit web link popup view model.
/// </summary>
/// <param name="webLinkRepository">Web link repository</param>
/// <param name="popupService">Popup service</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class AddEditWebLinkPopupViewModel(
    WebLinkRepository webLinkRepository,
    IPopupService popupService,
    ModalErrorHandler errorHandler)
    : ObservableObject, IQueryAttributable
{
    private WebLink? _webLink;
    private int _ownerId;
    private OwnerType _ownerType;

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
        if (query.ContainsKey("ownerId"))
        {
            _ownerId = Convert.ToInt32(query["ownerId"]);
        }
        if (query.ContainsKey("ownerType"))
        {
            _ownerType = (OwnerType)query["ownerType"];
        }
    }

    /// <summary>
    /// Load data.
    /// </summary>
    /// <param name="id">Record identifier.</param>
    private async Task LoadData(int id)
    {
        try
        {
            _webLink = await webLinkRepository.GetAsync(id);

            if (_webLink.IsNullOrNew())
            {
                errorHandler.HandleError(new Exception($"Web link with id {id} could not be found."));
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
    /// Save
    /// </summary>
    [RelayCommand]
    private async Task Save()
    {
        if (_webLink is null)
        {
            _webLink = new WebLink();
            _webLink.OwnerId = _ownerId;
            _webLink.OwnerType = _ownerType;
            _webLink.LinkType = 0;
        }

        _webLink.Name = Name;
        _webLink.URL = Url;
        _webLink.Note = Note;

        var webLinkId = await webLinkRepository.SaveItemAsync(_webLink);

        await popupService.ClosePopupAsync(Shell.Current);
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