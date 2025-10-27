namespace SmartGenealogy.Core.ViewModels.Popups;

/// <summary>
/// Select location popup view model.
/// </summary>
/// <param name="popupService">Popup service.</param>
public partial class SelectLocationPopupViewModel(
    IPopupService popupService)
    : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    List<FreeFormQueryResponse> _locations = new();

    [ObservableProperty]
    FreeFormQueryResponse? _selectedLocation;

    /// <summary>
    /// Apply attributes.
    /// </summary>
    /// <param name="query">Query</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("result"))
        {
            Locations  = (List<FreeFormQueryResponse>)query["result"];
            //LoadData(placeId).FireAndForgetSafeAsync();
        }
    }

    /// <summary>
    /// Save
    /// </summary>
    [RelayCommand]
    private async Task Save()
    {
        if (SelectedLocation is null)
            return;

        await popupService.ClosePopupAsync(Shell.Current, SelectedLocation);
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