namespace SmartGenealogy.Core.ViewModels.Popups;

/// <summary>
/// Add/edit place popup view model.
/// </summary>
/// <param name="placeService">Place service</param>
/// <param name="alertService">Alert service</param>
/// <param name="popupService">Popup service</param>
/// <param name="serviceProvider">Service provider</param>
/// <param name="locationIQService">LocationIQ service</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class AddEditPlacePopupViewModel(
    IPlaceService placeService,
    IAlertService alertService,
    IPopupService popupService,
    IServiceProvider serviceProvider,
    LocationIQService locationIQService,
    ModalErrorHandler errorHandler)
    : ObservableObject, IQueryAttributable
{
    private Place? _place;
    private int _masterId;

    [ObservableProperty]
    public string? _name = string.Empty;

    /// <summary>
    /// Apply attributes.
    /// </summary>
    /// <param name="query">Query</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("id"))
        {
            int placeId = Convert.ToInt32(query["id"]);
            LoadData(placeId).FireAndForgetSafeAsync();
        }
        if (query.ContainsKey("masterId"))
        {
            _masterId = Convert.ToInt32(query["masterId"]);
        }
    }

    /// <summary>
    /// Load data.
    /// </summary>
    /// <param name="id">Place identifier</param>
    private async Task LoadData(int id)
    {
        try
        {
            _place = await placeService.GetPlaceAsync(id);

            if (_place.IsNullOrNew())
            {
                errorHandler.HandleError(new Exception($"Place with id {id} could not be found."));
            }

            Name = _place!.Name;
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
        if (_place is null)
            _place = new Place();

        _place.Name = Name;
        if (_masterId != 0)
        {
            _place.PlaceType = PlaceType.Detail;
            _place.MasterId = _masterId;
        }
        if (_place.PlaceType == PlaceType.Master)
            _place.Normalized = Name;
        _place.Reverse = Name!.ReverseString();

        if (SmartGenealogySettings.GeocodePlaceOnSave)
        {
            var placeName = _place.Name + " " + ((_place.MasterPlace != null) ? _place.MasterPlace!.Name : string.Empty);
            locationIQService.LocationIQAPIKey = SmartGenealogySettings.LocationIQAPIKey;
            var result = await locationIQService.GetFreeFormQuery(placeName);

            if (result is null || result!.Count == 0)
            {
                await alertService.ShowAlertAsync("Geocode Place", "No results found for the place.", "OK");
            }
            else if (result!.Count == 1)
            {
                _place!.Latitude = decimal.Parse(result.FirstOrDefault()!.lat);
                _place!.Longitude = decimal.Parse(result.FirstOrDefault()!.lon);
            }
            else
            {
                var queryAttributes = new Dictionary<string, object>
                {
                    { "result", result }
                };

                IPopupResult<FreeFormQueryResponse> popupResult = await popupService.ShowPopupAsync<SelectLocationPopupViewModel, FreeFormQueryResponse>(
                    Shell.Current,
                    options: PopupOptions.Empty,
                    shellParameters: queryAttributes);

                if (popupResult is not null)
                {
                    _place!.Latitude = decimal.Parse(popupResult.Result!.lat);
                    _place!.Longitude = decimal.Parse(popupResult.Result!.lon);
                }
            }
        }

        var placeId = await placeService.SavePlaceAsync(_place);

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