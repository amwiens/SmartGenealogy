using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Places;

[QueryProperty(nameof(PlaceDetailId), "PlaceDetailId")]
[QueryProperty(nameof(IsEdited), "IsEdited")]
public partial class PlaceDetailViewModel : ObservableObject
{
    private readonly PlaceDetailService _placeDetailService;
    private readonly PlaceService _placeService;
    private readonly GeocodeService _geocodeService;

    [ObservableProperty]
    private int placeDetailId;

    [ObservableProperty]
    private PlaceDetail? placeDetail;

    [ObservableProperty]
    private Place? place;

    [ObservableProperty]
    private bool isEdited;

    public PlaceDetailViewModel(
        PlaceDetailService placeDetailService,
        PlaceService placeService,
        GeocodeService geocodeService)
    {
        _placeDetailService = placeDetailService;
        _placeService = placeService;
        _geocodeService = geocodeService;
    }

    private async Task LoadPlaceDetailAsync()
    {
        PlaceDetail = await _placeDetailService.GetPlaceDetailAsync(PlaceDetailId);
    }

    private async Task LoadPlaceAsync()
    {
        Place = await _placeService.GetPlaceAsync(PlaceDetail?.PlaceId ?? 0);
    }

    [RelayCommand]
    private async Task EditPlaceDetail()
    {
        if (Place == null)
            return;
        var parameters = new Dictionary<string, object>
        {
            { "PlaceDetail", PlaceDetail }
        };
        await Shell.Current.GoToAsync("EditPlaceDetailPage", parameters);
    }

    [RelayCommand]
    private async Task GeocodePlaceDetail()
    {
        var geocodeResult = await _geocodeService.GetPlaceAsync($"{PlaceDetail!.Address}, {Place!.City}, {Place!.State}");

        PlaceDetail.Latitude = geocodeResult.Latitude;
        PlaceDetail.Longitude = geocodeResult.Longitude;
        PlaceDetail.DateChanged = DateTime.Now;
        await _placeDetailService.UpdatePlaceDetailAsync(PlaceDetail);
        await LoadPlaceDetailAsync();
    }

    public async Task OnNavigatedToAsync()
    {
        var shouldRefresh = IsEdited || PlaceDetail is null;

        if (shouldRefresh)
        {
            await LoadPlaceDetailAsync();
            await LoadPlaceAsync();
            IsEdited = false;
        }
    }

    public Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }
}