using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Places;

public partial class AddPlaceViewModel : ObservableObject
{
    private readonly PlaceService _placeService;
    private readonly GeocodeService _geocodeService;

    [ObservableProperty]
    private string? city = string.Empty;

    [ObservableProperty]
    private string? county = string.Empty;

    [ObservableProperty]
    private string? state = string.Empty;

    [ObservableProperty]
    private string? country = string.Empty;

    [ObservableProperty]
    private string? notes = string.Empty;

    public AddPlaceViewModel(PlaceService placeService, GeocodeService geocodeService)
    {
        _placeService = placeService;
        _geocodeService = geocodeService;
    }

    [RelayCommand]
    private async Task SavePlaceAsync()
    {
        if (string.IsNullOrWhiteSpace(City) || string.IsNullOrWhiteSpace(State))
            return;

        var geocodeResult = await _geocodeService.GetPlaceAsync($"{City}, {State}");

        var place = new Place
        {
            City = (!string.IsNullOrEmpty(geocodeResult.City)) ? geocodeResult.City : City,
            County = (!string.IsNullOrEmpty(geocodeResult.County)) ? geocodeResult.County : County,
            State = (!string.IsNullOrEmpty(geocodeResult.State)) ? geocodeResult.State : State,
            Country = (!string.IsNullOrEmpty(geocodeResult.Country)) ? geocodeResult.Country : Country,
            Notes = Notes,
            Latitude = geocodeResult.Latitude,
            Longitude = geocodeResult.Longitude,
            DateChanged = DateTime.Now
        };

        var placesBaseDirectory = SettingsManager.LoadSettings().PlacesBaseDirectory;

        if (string.IsNullOrEmpty(placesBaseDirectory))
        {
            await Application.Current!.Windows[0].Page!.DisplayAlert(
                "Add directory",
                $"Places base directory is not set",
                "Ok");

            return;
        }

        var directoryPath = Path.Combine(placesBaseDirectory, place.Country!, place.State, place.County!, place.City);

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        await _placeService.AddPlaceAsync(place);

        var parameters = new Dictionary<string, object>
        {
            { "IsEdited", true }
        };
        await Shell.Current.GoToAsync("..", true, parameters);
    }
}