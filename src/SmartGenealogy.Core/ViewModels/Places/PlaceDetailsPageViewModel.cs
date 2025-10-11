using SmartGenealogy.Data.Repositories;

namespace SmartGenealogy.Core.ViewModels.Places;

/// <summary>
/// Place details page view model
/// </summary>
/// <param name="placeRepository">Place repository</param>
/// <param name="mediaLinkRepository">Media link repository</param>
/// <param name="popupService">Popup service</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class PlaceDetailsPageViewModel(
    PlaceRepository placeRepository,
    MediaLinkRepository mediaLinkRepository,
    IPopupService popupService,
    ModalErrorHandler errorHandler)
    : ObservableObject, IQueryAttributable
{
    private Place? _place;

    [ObservableProperty]
    private string? _name = string.Empty;

    [ObservableProperty]
    private decimal _latitude = 0;

    [ObservableProperty]
    private decimal _longitude = 0;

    [ObservableProperty]
    private string? _note = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Place> _placeDetails = [];

    [ObservableProperty]
    private Place? _selectedPlaceDetail;

    [ObservableProperty]
    private ObservableCollection<MediaLink> _mediaLinks = [];

    [ObservableProperty]
    private MediaLink? _selectedMediaLink;

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
    /// <param name="id">Place identifier.</param>
    private async Task LoadData(int id)
    {
        try
        {
            _place = await placeRepository.GetAsync(id);

            if (_place.IsNullOrNew())
            {
                errorHandler.HandleError(new Exception($"Place with id {id} could not be found."));
                return;
            }

            Name = _place.Name;
            Latitude = _place.Latitude;
            Longitude = _place.Longitude;
            Note = _place.Note;
            PlaceDetails = new ObservableCollection<Place>(_place.PlaceDetails!);
            MediaLinks = new ObservableCollection<MediaLink>(_place.MediaLinks!);
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Edit place.
    /// </summary>
    [RelayCommand]
    private async Task EditPlace()
    {
        var queryAttributes = new Dictionary<string, object>
        {
            { "id", _place!.Id }
        };

        await popupService.ShowPopupAsync<AddEditPlacePopupViewModel>(
            Shell.Current,
            options: PopupOptions.Empty,
            shellParameters: queryAttributes);
    }

    /// <summary>
    /// Delete place.
    /// </summary>
    [RelayCommand]
    private async Task DeletePlace()
    {
        try
        {
            await placeRepository.DeleteItemAsync(_place!);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Add place details.
    /// </summary>
    [RelayCommand]
    private async Task AddPlaceDetails()
    {
        var queryAttributes = new Dictionary<string, object>
        {
            { "masterId", _place!.Id }
        };

        await popupService.ShowPopupAsync<AddEditPlacePopupViewModel>(
            Shell.Current,
            options: PopupOptions.Empty,
            shellParameters: queryAttributes);
    }

    /// <summary>
    /// Open place details
    /// </summary>
    [RelayCommand]
    private async Task OpenPlaceDetails()
    {
        if (SelectedPlaceDetail is not null)
            await Shell.Current.GoToAsync($"placeDetails?id={SelectedPlaceDetail.Id}");
    }

    /// <summary>
    /// Add media link.
    /// </summary>
    [RelayCommand]
    private async Task AddMediaLink()
    {
        await mediaLinkRepository.SaveItemAsync(new MediaLink
        {
            MultimediaId = 12,
            OwnerType = OwnerType.Place,
            OwnerId = _place!.Id,
            IsPrimary = true,
            Comments = "Test"
        });
    }

    /// <summary>
    /// Open multimedia details
    /// </summary>
    [RelayCommand]
    private async Task OpenMultimedia()
    {
        if (SelectedMediaLink is not null)
            await Shell.Current.GoToAsync($"multimediaDetails?id={SelectedMediaLink.MultimediaId}");
    }
}