using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Enums;
using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Places;

[QueryProperty(nameof(PlaceId), "PlaceId")]
[QueryProperty(nameof(IsEdited), "IsEdited")]
public partial class PlaceViewModel : ObservableObject
{
    private readonly PlaceService _placeService;
    private readonly PlaceDetailService _placeDetailService;
    private readonly GeocodeService _geocodeService;

    [ObservableProperty]
    private int placeId;

    [ObservableProperty]
    private Place? place;

    [ObservableProperty]
    private ObservableCollection<PlaceDetail> _placeDetails;

    [ObservableProperty]
    private bool isEdited;

    public PlaceViewModel(
        PlaceService placeService,
        PlaceDetailService placeDetailService,
        GeocodeService geocodeService)
    {
        _placeService = placeService;
        _placeDetailService = placeDetailService;
        _geocodeService = geocodeService;
    }

    private async Task LoadPlaceAsync()
    {
        Place = await _placeService.GetPlaceAsync(PlaceId);
    }

    private async Task LoadPlaceDetailAsync()
    {
        if (Place == null)
            return;
        var placeDetails = await _placeDetailService.GetPlaceDetailsByPlaceIdAsync(PlaceId);
        PlaceDetails = new ObservableCollection<PlaceDetail>(placeDetails);
    }

    [RelayCommand]
    private async Task EditPlace()
    {
        if (Place == null)
            return;
        var parameters = new Dictionary<string, object>
        {
            { "Place", Place }
        };
        await Shell.Current.GoToAsync("EditPlacePage", parameters);
    }

    [RelayCommand]
    private async Task AddMedia()
    {
        if (Place == null)
            return;
        var parameters = new Dictionary<string, object>
        {
            { "OwnerType", OwnerType.Place },
            { "OwnerId", Place.Id }
        };
        await Shell.Current.GoToAsync("AddMediaPage", parameters);
    }

    [RelayCommand]
    private async Task GeocodePlace()
    {
        var geocodeResult = await _geocodeService.GetPlaceAsync($"{Place!.City}, {Place!.State}");

        Place.Latitude = geocodeResult.Latitude;
        Place.Longitude = geocodeResult.Longitude;
        Place.DateChanged = DateTime.Now;
        await _placeService.UpdatePlaceAsync(Place);
        await LoadPlaceAsync();
    }

    [RelayCommand]
    private async Task AddPlaceDetail()
    {
        if (Place == null)
            return;
        var parameters = new Dictionary<string, object>
        {
            { "Place", Place }
        };
        await Shell.Current.GoToAsync("AddPlaceDetailPage", parameters);
    }

    [RelayCommand]
    private async Task GoToPlaceDetail(PlaceDetail placeDetail)
    {
        if (placeDetail == null)
            return;
        var parameters = new Dictionary<string, object>
        {
            { "PlaceDetailId", placeDetail.Id }
        };
        await Shell.Current.GoToAsync("PlaceDetailPage", parameters);
    }

    [RelayCommand]
    private async Task DeletePlaceDetail(PlaceDetail placeDetail)
    {
        if (placeDetail == null)
            return;

        await _placeDetailService.DeletePlaceDetailAsync(placeDetail);
        PlaceDetails.Remove(placeDetail);
    }

    public async Task OnNavigatedToAsync()
    {
        var shouldRefresh = IsEdited || Place is null;

        if (shouldRefresh)
        {
            await LoadPlaceAsync();
            await LoadPlaceDetailAsync();
            IsEdited = false;
        }
    }

    public Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }
}