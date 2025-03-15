using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Places;

[QueryProperty(nameof(PlaceDetail), "PlaceDetail")]
public partial class EditPlaceDetailViewModel : ObservableObject
{
    private readonly PlaceDetailService _placeDetailService;
    private string originalName = string.Empty;
    private string originalAddress = string.Empty;
    private string originalNotes = string.Empty;

    [ObservableProperty]
    PlaceDetail? placeDetail;

    partial void OnPlaceDetailChanged(PlaceDetail? value)
    {
        if (value != null)
        {
            originalName = value.Name ?? string.Empty;
            originalAddress = value.Address ?? string.Empty;
            originalNotes = value.Notes ?? string.Empty;
        }
    }

    public EditPlaceDetailViewModel(PlaceDetailService placeDetailService)
    {
        _placeDetailService = placeDetailService;
    }

    private bool HasPlaceChanged()
    {
        if (PlaceDetail == null)
            return false;

        return !string.Equals(PlaceDetail.Name ?? string.Empty, originalName) ||
            !string.Equals(PlaceDetail.Address ?? string.Empty, originalAddress) ||
            !string.Equals(PlaceDetail.Notes ?? string.Empty, originalNotes);
    }

    [RelayCommand]
    async Task SavePlaceDetail()
    {
        if (PlaceDetail == null)
            return;

        var hasPlaceChanged = HasPlaceChanged();
        if (hasPlaceChanged)
        {
            PlaceDetail.DateChanged = DateTime.Now;
            await _placeDetailService.UpdatePlaceDetailAsync(PlaceDetail);
        }

        var parameters = new Dictionary<string, object>
        {
            { "IsEdited", hasPlaceChanged }
        };
        await Shell.Current.GoToAsync("..", true, parameters);
    }

    [RelayCommand]
    async Task DeletePlaceDetail()
    {
        if (PlaceDetail == null)
            return;

        bool answer = await Application.Current.MainPage.DisplayAlert(
            "Delete Place",
            $"You you sure you want to delete {PlaceDetail.Name}?",
            "Yes", "No");

        if (answer)
        {
            await _placeDetailService.DeletePlaceDetailAsync(PlaceDetail);

            var parameters = new Dictionary<string, object>
            {
                { "IsEdited", true },
                { "PlaceId", PlaceDetail.PlaceId }
            };
            await Shell.Current.GoToAsync("../..", true, parameters);
        }
    }
}