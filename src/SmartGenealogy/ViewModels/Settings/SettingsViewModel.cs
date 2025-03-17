using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Settings;

public partial class SettingsViewModel : ObservableObject, INavigationAwareAsync
{
    [ObservableProperty]
    private AppSettings settings;

    [ObservableProperty]
    private string? tesseractLanguageFileLocation;

    [ObservableProperty]
    private string? geocodioApiKey;

    [ObservableProperty]
    private string? placesBaseDirectory;

    public SettingsViewModel()
    {
        Settings = new AppSettings();
    }

    [RelayCommand]
    private async Task GoToImageSettings()
    {
        await Shell.Current.GoToAsync("ImageSettingsPage");
    }

    [RelayCommand]
    private async Task GoToPlaceSettings()
    {
        await Shell.Current.GoToAsync("PlaceSettingsPage");
    }

    [RelayCommand]
    private async Task GoToAISettings()
    {
        await Shell.Current.GoToAsync("AISettingsPage");
    }

    public async Task OnNavigatedToAsync()
    {
        Settings = SettingsManager.LoadSettings();
        TesseractLanguageFileLocation = Settings.TesseractLanguageFileLocation;
        GeocodioApiKey = Settings.GeocodioApiKey;
        PlacesBaseDirectory = Settings.PlacesBaseDirectory;
    }

    public Task OnNavigatedFromAsync()
    {
        Settings.TesseractLanguageFileLocation = TesseractLanguageFileLocation;
        Settings.GeocodioApiKey = GeocodioApiKey;
        Settings.PlacesBaseDirectory = PlacesBaseDirectory;
        SettingsManager.SaveSettings(Settings);
        return Task.CompletedTask;
    }
}