namespace SmartGenealogy.Core.ViewModels.Popups;

/// <summary>
/// Add/edit place coordinates popup view model.
/// </summary>
/// <param name="placeService">Place service</param>
/// <param name="popupService">Popup service</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class AddEditPlaceCoordinatesPopupViewModel(
    IPlaceService placeService,
    IPopupService popupService,
    ModalErrorHandler errorHandler)
    : ObservableObject, IQueryAttributable
{
    private Place? _place;

    [ObservableProperty]
    public string _latitude = string.Empty;

    [ObservableProperty]
    public string _longitude = string.Empty;

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

            Latitude = _place!.Latitude.ToString();
            Longitude = _place!.Longitude.ToString();
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Handles the text changed event of the Latitude text box.
    /// </summary>
    [RelayCommand]
    private void LatitudeChanged()
    {
        if (Latitude.Contains(','))
        {
            var coordinates = Latitude.Split(',');
            Latitude = coordinates[0].Trim();
            Longitude = coordinates[1].Trim();
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

        if (decimal.TryParse(Latitude, out var latitude))
            _place.Latitude = latitude;
        if (decimal.TryParse(Longitude, out var longitude))
            _place.Longitude = longitude;

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