using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Enums;
using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Places;

[QueryProperty(nameof(PlaceDetail), "PlaceDetail")]
public partial class EditPlaceDetailViewModel : ObservableObject
{
    private readonly PlaceDetailService _placeDetailService;
    private string originalName = string.Empty;
    private string originalPlaceDetailType = string.Empty;
    private string originalAddress = string.Empty;
    private string originalNotes = string.Empty;

    [ObservableProperty]
    private PlaceDetail? placeDetail;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string type;

    [ObservableProperty]
    private string? address;

    [ObservableProperty]
    private string? notes;

    public List<string> PlaceDetailTypes { get; } = Enum.GetNames<PlaceDetailType>().Order().ToList();

    public bool isInitialLoad = true;

    partial void OnPlaceDetailChanged(PlaceDetail? value)
    {
        if (value != null)
        {
            if (isInitialLoad)
            {
                Name = PlaceDetail!.Name;
                Type = Enum.GetName(typeof(PlaceDetailType), PlaceDetail!.Type)!;
                Address = PlaceDetail!.Address;
                Notes = PlaceDetail!.Notes;
                isInitialLoad = false;
            }

            originalName = Name ?? string.Empty;
            originalPlaceDetailType = Type ?? string.Empty;
            originalAddress = Address ?? string.Empty;
            originalNotes = Notes ?? string.Empty;
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
            !string.Equals(PlaceDetail.Type, originalPlaceDetailType) ||
            !string.Equals(PlaceDetail.Address ?? string.Empty, originalAddress) ||
            !string.Equals(PlaceDetail.Notes ?? string.Empty, originalNotes);
    }

    [RelayCommand]
    private async Task SavePlaceDetail()
    {
        if (PlaceDetail == null)
            return;

        var hasPlaceChanged = HasPlaceChanged();
        if (hasPlaceChanged)
        {
            PlaceDetail.Name = Name;
            PlaceDetail.Type = Enum.TryParse<PlaceDetailType>(@Type, out var parsedPlaceDetailType) ? parsedPlaceDetailType : PlaceDetailType.Other;
            PlaceDetail.Address = Address;
            PlaceDetail.Notes = Notes;
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
    private async Task DeletePlaceDetail()
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