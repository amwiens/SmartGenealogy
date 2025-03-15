using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Places;

[QueryProperty(nameof(Place), "Place")]
public partial class AddPlaceDetailViewModel : ObservableObject
{
    private readonly PlaceDetailService _placeDetailService;
    private readonly GeocodeService _geocodeService;

    [ObservableProperty]
    private Place place;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? address;

    [ObservableProperty]
    private string? notes;

    public AddPlaceDetailViewModel(PlaceDetailService placeDetailService, GeocodeService geocodeService)
    {
        _placeDetailService = placeDetailService;
        _geocodeService = geocodeService;
    }

    [RelayCommand]
    private async Task SavePlaceDetailAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
            return;

        var geocodeResult = await _geocodeService.GetPlaceAsync($"{Address}, {Place.City}, {Place.State}");

        var placeDetail = new Models.PlaceDetail
        {
            PlaceId = Place.Id,
            Name = Name,
            Address = Address,
            Notes = Notes,
            Latitude = geocodeResult.Latitude,
            Longitude = geocodeResult.Longitude,
            DateChanged = DateTime.Now
        };

        await _placeDetailService.AddPlaceDetailAsync(placeDetail);

        var parameters = new Dictionary<string, object>
        {
            { "IsEdited", true }
        };
        await Shell.Current.GoToAsync("..", true, parameters);
    }
}