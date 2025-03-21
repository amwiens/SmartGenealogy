﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Places;

[QueryProperty(nameof(Place), "Place")]
public partial class EditPlaceViewModel : ObservableObject
{
    private readonly PlaceService _placeService;
    private readonly PlaceDetailService _placeDetailService;
    private readonly GeocodeService _geocodeService;
    private string originalCity = string.Empty;
    private string originalCounty = string.Empty;
    private string originalState = string.Empty;
    private string originalCountry = string.Empty;

    [ObservableProperty]
    private Place? place;

    partial void OnPlaceChanged(Place? value)
    {
        if (value != null)
        {
            originalCity = value.City ?? string.Empty;
            originalCounty = value.County ?? string.Empty;
            originalState = value.State ?? string.Empty;
            originalCountry = value.Country ?? string.Empty;
        }
    }

    public EditPlaceViewModel(
        PlaceService placeService,
        PlaceDetailService placeDetailService,
        GeocodeService geocodeService)
    {
        _placeService = placeService;
        _placeDetailService = placeDetailService;
        _geocodeService = geocodeService;
    }

    private bool HasPlaceChanged()
    {
        if (Place == null)
            return false;

        return !string.Equals(Place.City ?? string.Empty, originalCity) ||
            !string.Equals(Place.County ?? string.Empty, originalCounty) ||
            !string.Equals(Place.State ?? string.Empty, originalState) ||
            !string.Equals(Place.Country ?? string.Empty, originalCountry);
    }

    [RelayCommand]
    private async Task SavePlace()
    {
        if (Place == null)
            return;

        var hasPlaceChanged = HasPlaceChanged();
        if (hasPlaceChanged)
        {
            Place.DateChanged = DateTime.Now;
            await _placeService.UpdatePlaceAsync(Place);
        }

        var parameters = new Dictionary<string, object>
        {
            { "IsEdited", hasPlaceChanged }
        };
        await Shell.Current.GoToAsync("..", true, parameters);
    }

    [RelayCommand]
    private async Task DeletePlace()
    {
        if (Place == null)
            return;

        bool answer = await Application.Current!.Windows[0].Page!.DisplayAlert(
            "Delete Place",
            $"You you sure you want to delete {Place.City}, {Place.County}, {Place.State}, {Place.Country}?",
            "Yes", "No");

        if (answer)
        {
            var placeDetails = await _placeDetailService.GetPlaceDetailsByPlaceIdAsync(Place.Id);

            foreach (var placeDetail in placeDetails)
            {
                await _placeDetailService.DeletePlaceDetailAsync(placeDetail);
            }

            await _placeService.DeletePlaceAsync(Place);

            var parameters = new Dictionary<string, object>
            {
                { "IsEdited", true }
            };
            await Shell.Current.GoToAsync("../..", true, parameters);
        }
    }
}