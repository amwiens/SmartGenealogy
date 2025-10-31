namespace SmartGenealogy.Core.ViewModels.Places;

/// <summary>
/// Place page view model.
/// </summary>
/// <param name="placeService">Place service</param>
/// <param name="mediaLinkRepository">Media link repository</param>
/// <param name="webLinkLinkRepository">Weblink Link repository</param>
/// <param name="alertService">Alert service</param>
/// <param name="popupService">Popup service</param>
/// <param name="locationIQService">LocationIQ service</param>
/// <param name="errorHandler">Modal error handler</param>
public partial class PlacePageViewModel(
    IPlaceService placeService,
    MediaLinkRepository mediaLinkRepository,
    WebLinkLinkRepository webLinkLinkRepository,
    IAlertService alertService,
    IPopupService popupService,
    LocationIQService locationIQService,
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
    private DateTime? _dateAdded;

    [ObservableProperty]
    private DateTime? _dateChanged;

    [ObservableProperty]
    private ObservableCollection<Place> _placeDetails = [];

    [ObservableProperty]
    private ObservableCollection<MediaLink> _mediaLinks = [];

    [ObservableProperty]
    private ObservableCollection<WebLinkLink> _webLinks = [];

    [ObservableProperty]
    private Place? _selectedPlaceDetail;

    [ObservableProperty]
    private MediaLink? _selectedMediaLink;

    [ObservableProperty]
    private WebLinkLink? _selectedWebLink;

    [ObservableProperty]
    private Mapsui.Map? _map;

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
            _place = await placeService.GetPlaceAsync(id);

            if (_place.IsNullOrNew())
            {
                errorHandler.HandleError(new Exception($"Place with id {id} could not be found."));
                return;
            }

            Name = _place.Name;
            Latitude = _place.Latitude;
            Longitude = _place.Longitude;
            Note = _place.Note;
            PlaceDetails = new ObservableCollection<Place>(_place.PlaceDetails!.OrderBy(x => x.Name));
            MediaLinks = new ObservableCollection<MediaLink>(_place.MediaLinks!);
            WebLinks = new ObservableCollection<WebLinkLink>(_place.WebLinks!);
            DateAdded = _place.DateAdded.ToLocalTime();
            DateChanged = _place.DateChanged.ToLocalTime();

            Map = new Mapsui.Map();
            Map.Layers.Add(OpenStreetMap.CreateTileLayer());
            var features = new List<PointFeature>
            {
                new PointFeature(SphericalMercator.FromLonLat((double)Longitude, (double)Latitude)),
            };
            foreach (var detail in PlaceDetails)
            {
                if (detail.Latitude != 0 && detail.Longitude != 0)
                {
                    features.Add(new PointFeature(SphericalMercator.FromLonLat((double)detail.Longitude, (double)detail.Latitude)));
                }
            }
            Map.Layers.Add(new MemoryLayer
            {
                Name = "Place Layer",
                Features = features,
                //Style = new ImageStyle { }
            });
            var center = new MPoint(SphericalMercator.FromLonLat((double)Longitude, (double)Latitude));
            Map.Navigator.CenterOnAndZoomTo(center, Map.Navigator.Resolutions[12]);
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
            var isConfirmed = await alertService.ShowAlertAsync("Delete place", "Are you sure you want to delete this place?", "Yes", "No");
            if (isConfirmed)
            {
                var isDeleted = await placeService.DeletePlaceAsync(_place!);
                if (isDeleted)
                    await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Geocode place
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task GeocodePlace()
    {
        locationIQService.LocationIQAPIKey = SmartGenealogySettings.LocationIQAPIKey;
        var result = await locationIQService.GetFreeFormQuery(_place!.Name!);

        if (result!.Count == 0)
        {
            await alertService.ShowAlertAsync("Geocode Place", "No results found for the place.", "Ok");
        }
        else if (result!.Count == 1)
        {
            _place!.Latitude = decimal.Parse(result.FirstOrDefault()!.lat);
            _place!.Longitude = decimal.Parse(result.FirstOrDefault()!.lon);
            await placeService.SavePlaceAsync(_place!);
            LoadData(_place!.Id).FireAndForgetSafeAsync();
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

    /// <summary>
    /// Edit the coordinates for a place.
    /// </summary>
    [RelayCommand]
    private async Task EditCoordinates()
    {
        var queryAttributes = new Dictionary<string, object>
        {
            { "id", _place!.Id }
        };

        await popupService.ShowPopupAsync<AddEditPlaceCoordinatesPopupViewModel>(
            Shell.Current,
            options: PopupOptions.Empty,
            shellParameters: queryAttributes);
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
    /// Add new media link.
    /// </summary>
    [RelayCommand]
    private async Task AddNewMediaLink()
    {
        try
        {
            if (Shell.Current is Shell shell)
            {
                var result = await popupService.ShowPopupAsync<AddEditMultimediaPopupViewModel, int>(shell);

                if (result.Result != 0)
                {
                    var ownerType = OwnerType.Place;
                    var mediaLinks = await mediaLinkRepository.ListAsync(ownerType, _place!.Id);

                    if (!mediaLinks!.Where(x => x.MultimediaId == result.Result).Any())
                    {
                        await mediaLinkRepository.SaveItemAsync(new MediaLink
                        {
                            MultimediaId = result.Result,
                            OwnerType = ownerType,
                            OwnerId = _place!.Id,
                            IsPrimary = false,
                            Comments = string.Empty
                        });
                        LoadData(_place!.Id).FireAndForgetSafeAsync();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Add existing media link.
    /// </summary>
    [RelayCommand]
    private async Task AddExistingMediaLink()
    {
        try
        {
            if (Shell.Current is Shell shell)
            {
                var result = await popupService.ShowPopupAsync<SelectMultimediaPopupViewModel, int>(shell);

                if (result.Result != 0)
                {
                    var ownerType = OwnerType.Place;
                    var mediaLinks = await mediaLinkRepository.ListAsync(ownerType, _place!.Id);

                    if (!mediaLinks!.Where(x => x.MultimediaId == result.Result).Any())
                    {
                        await mediaLinkRepository.SaveItemAsync(new MediaLink
                        {
                            MultimediaId = result.Result,
                            OwnerType = OwnerType.Place,
                            OwnerId = _place!.Id,
                            IsPrimary = false,
                            Comments = string.Empty
                        });
                        LoadData(_place!.Id).FireAndForgetSafeAsync();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Delete media link
    /// </summary>
    [RelayCommand]
    private async Task DeleteMediaLink()
    {
        try
        {
            if (SelectedMediaLink is not null)
            {
                var isConfirmed = await alertService.ShowAlertAsync("Delete media link", "Are you sure you want to delete this media link?", "Yes", "No");
                if (isConfirmed)
                {
                    await mediaLinkRepository.DeleteItemAsync(SelectedMediaLink);
                    LoadData(_place!.Id).FireAndForgetSafeAsync();
                }
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
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

    /// <summary>
    /// Add new web link.
    /// </summary>
    [RelayCommand]
    private async Task AddNewWebLink()
    {
        try
        {
            var queryAttributes = new Dictionary<string, object>
            {
                { "ownerId", _place!.Id },
                { "ownerType", OwnerType.Place }
            };

            if (Shell.Current is Shell shell)
            {
                var result = await popupService.ShowPopupAsync<AddEditWebLinkPopupViewModel>(
                    shell,
                    options: PopupOptions.Empty,
                    shellParameters: queryAttributes);
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Add an existing web link.
    /// </summary>
    [RelayCommand]
    private async Task AddExistingWebLink()
    {
        try
        {
            if (Shell.Current is Shell shell)
            {
                var result = await popupService.ShowPopupAsync<SelectWebLinkPopupViewModel, int>(shell);

                if (result.Result != 0)
                {
                    await webLinkLinkRepository.SaveItemAsync(new WebLinkLink
                    {
                        WebLinkId = result.Result,
                        OwnerType = OwnerType.Place,
                        OwnerId = _place!.Id,
                    });
                    LoadData(_place!.Id).FireAndForgetSafeAsync();
                }
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Delete web link.
    /// </summary>
    [RelayCommand]
    private async Task DeleteWebLink()
    {
        try
        {
            if (SelectedWebLink is not null)
            {
                var isConfirmed = await alertService.ShowAlertAsync("Delete web link", "Are you sure you want to delete this web link?", "Yes", "No");
                if (isConfirmed)
                {
                    await webLinkLinkRepository.DeleteItemAsync(SelectedWebLink);
                    LoadData(_place!.Id).FireAndForgetSafeAsync();
                }
            }
        }
        catch (Exception ex)
        {
            errorHandler.HandleError(ex);
        }
    }

    /// <summary>
    /// Open web link.
    /// </summary>
    [RelayCommand]
    private async Task OpenWebLink()
    {
    }
}