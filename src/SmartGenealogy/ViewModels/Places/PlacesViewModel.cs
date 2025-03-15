using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Places;

[QueryProperty(nameof(IsEdited), "IsEdited")]
public partial class PlacesViewModel : ObservableObject, INavigationAwareAsync
{
    private readonly PlaceService _placeService;

    [ObservableProperty]
    private ObservableCollection<Place> _places;

    [ObservableProperty]
    private bool isEdited;

    public PlacesViewModel(PlaceService placeService)
    {
        _placeService = placeService;
        Places = new ObservableCollection<Place>();
    }

    private async Task LoadPlacesAsync()
    {
        var places = await _placeService.GetPlacesAsync();
        Places.Clear();
        foreach (var place in places)
        {
            Places.Add(place);
        }
    }

    [RelayCommand]
    private async Task AddPlace()
    {
        await Shell.Current.GoToAsync("AddPlacePage");
    }

    [RelayCommand]
    async Task GoToPlace(Place place)
    {
        if (place == null)
            return;
        var parameters = new Dictionary<string, object>
        {
            { "PlaceId", place.Id }
        };
        await Shell.Current.GoToAsync("PlacePage", parameters);
    }

    [RelayCommand]
    async Task DeletePlace(Place place)
    {
        if (place == null)
            return;

        await _placeService.DeletePlaceAsync(place);
        Places.Remove(place);
    }

    public async Task OnNavigatedToAsync()
    {
        var shouldRefresh = IsEdited || Places.Count == 0;

        if (shouldRefresh)
        {
            await LoadPlacesAsync();
            IsEdited = false;
        }
    }

    public Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }
}