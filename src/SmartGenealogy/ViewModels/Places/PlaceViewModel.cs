﻿using System.Collections.ObjectModel;

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

    [ObservableProperty]
    private Mapsui.Map map;

    public PlaceViewModel(
        PlaceService placeService,
        PlaceDetailService placeDetailService,
        GeocodeService geocodeService)
    {
        _placeService = placeService;
        _placeDetailService = placeDetailService;
        _geocodeService = geocodeService;

        Map = new Mapsui.Map();
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

    partial void OnPlaceChanged(Place? value)
    {
        Map.Layers.Add(OpenStreetMap.CreateTileLayer());
        if (value!.Longitude != null && value.Latitude != null)
        {
            var pinLayer = new MemoryLayer
            {
                Name = "Pins",
                Features = new ObservableCollection<IFeature>
                {
                    new PointFeature(SphericalMercator.FromLonLat((double)value!.Longitude!, (double)value!.Latitude!).ToMPoint()) { ["Label"] = value.City },
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
            Map.Navigator.CenterOnAndZoomTo(Map.Layers[1].Extent!.Centroid, 12);
            Map.RefreshGraphics();
        }
    }

    partial void OnPlaceDetailsChanged(ObservableCollection<PlaceDetail> value)
    {
        var placeDetailsFeatures = new ObservableCollection<IFeature>();
        foreach (var placeDetail in PlaceDetails)
        {
            if (placeDetail.Latitude != null && placeDetail.Longitude != null)
            {
                placeDetailsFeatures.Add(new PointFeature(SphericalMercator.FromLonLat((double)placeDetail.Longitude!, (double)placeDetail.Latitude!).ToMPoint())
                {
                    ["Label"] = placeDetail.Name
                });
            }
        }
        var placeDetailsLayer = new MemoryLayer
        {
            Name = "PlaceDetails",
            Features = placeDetailsFeatures,
            Style = new SymbolStyle
            {
                SymbolScale = 0.5,
                Fill = new Mapsui.Styles.Brush(Mapsui.Styles.Color.Blue),
                Outline = new Pen(Mapsui.Styles.Color.Black)
            }
        };
        Map.Layers.Add(placeDetailsLayer);
        Map.Widgets.Add(new ZoomInOutWidget { MarginX = 10, MarginY = 20 });
        Map.Navigator.CenterOnAndZoomTo(Map.Layers[2].Extent!.Centroid, 12);
        Map.RefreshGraphics();
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