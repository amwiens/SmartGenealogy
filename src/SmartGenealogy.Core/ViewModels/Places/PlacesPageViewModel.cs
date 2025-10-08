namespace SmartGenealogy.Core.ViewModels.Places;

/// <summary>
/// Places page view model.
/// </summary>
/// <param name="placeRepository">Place repository</param>
/// <param name="popupService">Popup service</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class PlacesPageViewModel(
    PlaceRepository placeRepository,
    IPopupService popupService,
    ModalErrorHandler errorHandler) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Place> _places = [];

    [ObservableProperty]
    private Place? _selectedItem;

    /// <summary>
    /// Appearing
    /// </summary>
    [RelayCommand]
    private async Task Appearing()
    {
        Places = new ObservableCollection<Place>(await placeRepository.ListAsync());
    }

    /// <summary>
    /// Add place.
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task AddPlace()
    {
        try
        {
            if (Shell.Current is Shell shell)
            {
                var result = await popupService.ShowPopupAsync<AddEditPlacePopupViewModel>(shell);
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Open place details
    /// </summary>
    [RelayCommand]
    private async Task OpenPlaceDetails()
    {

    }
}