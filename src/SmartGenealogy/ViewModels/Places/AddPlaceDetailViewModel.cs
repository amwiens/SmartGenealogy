using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Enums;
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
    private string type;

    [ObservableProperty]
    private string? address;

    [ObservableProperty]
    private string? notes;

    public List<string> PlaceDetailTypes { get; } = Enum.GetNames<PlaceDetailType>().Order().ToList();

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

        var placeDetailType = Enum.TryParse<PlaceDetailType>(@Type, out var parsedPlaceDetailType) ? parsedPlaceDetailType : PlaceDetailType.Other;
        var placeDetail = new PlaceDetail
        {
            PlaceId = Place.Id,
            Name = Name,
            Type = placeDetailType,
            Address = Address,
            Notes = Notes,
            Latitude = geocodeResult.Latitude,
            Longitude = geocodeResult.Longitude,
            DateChanged = DateTime.Now
        };

        var directoryPath = Path.Combine(@"C:\Genealogy\Research\Places", Place.Country!, Place.State!, Place.County!, Place.City!, Name);

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        await _placeDetailService.AddPlaceDetailAsync(placeDetail);

        switch (placeDetailType)
        {
            case PlaceDetailType.Cemetery:
            case PlaceDetailType.Church:
            case PlaceDetailType.Newspaper:
            case PlaceDetailType.Other:
            case PlaceDetailType.Home:
            case PlaceDetailType.Business:
            case PlaceDetailType.Hospital:
                break;
            case PlaceDetailType.School:
                var yearbookDirectoryPath = Path.Combine(directoryPath, "Yearbooks");
                if (!Directory.Exists(yearbookDirectoryPath))
                    Directory.CreateDirectory(yearbookDirectoryPath);
                break;

            default:
                throw new ArgumentOutOfRangeException("PlaceDetailType");
        }

        var parameters = new Dictionary<string, object>
        {
            { "IsEdited", true }
        };
        await Shell.Current.GoToAsync("..", true, parameters);
    }
}