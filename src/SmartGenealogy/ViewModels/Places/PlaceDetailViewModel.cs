using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;

using Mapsui.Styles;

using Mapsui.Tiling;
using Mapsui.Widgets.Zoom;

using SmartGenealogy.Enums;
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

    [ObservableProperty]
    private Mapsui.Map map;

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


    partial void OnPlaceDetailChanged(PlaceDetail? value)
    {
        Map = new Mapsui.Map();
        Map.Layers.Add(OpenStreetMap.CreateTileLayer());
        if (value!.Longitude != null && value.Latitude != null)
        {
            var pinLayer = new MemoryLayer
            {
                Name = "Pins",
                Features = new ObservableCollection<IFeature>
                {
                    new PointFeature(SphericalMercator.FromLonLat((double)value!.Longitude!, (double)value!.Latitude!).ToMPoint()) { ["Label"] = value.Name },
                },
                Style = new SymbolStyle
                {
                    SymbolScale = 0.5,
                    Fill = new Mapsui.Styles.Brush(Mapsui.Styles.Color.Red),
                    Outline = new Pen(Mapsui.Styles.Color.Black)
                }
            };
            Map.Layers.Add(pinLayer);
            Map.Widgets.Add(new ZoomInOutWidget { MarginX = 10, MarginY = 20 });
            Map.Navigator.CenterOnAndZoomTo(SphericalMercator.FromLonLat((double)value!.Longitude!, (double)value!.Latitude!).ToMPoint(), 12);
            Map.RefreshGraphics();
        }
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
    private async Task AddMedia()
    {
        if (PlaceDetail == null)
            return;
        var parameters = new Dictionary<string, object>
        {
            { "OwnerType", OwnerType.PlaceDetail },
            { "OwnerId", PlaceDetail.Id }
        };
        await Shell.Current.GoToAsync("AddMediaPage", parameters);
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