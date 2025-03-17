using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Settings;

public partial class PlaceSettingsViewModel : ObservableObject, INavigationAwareAsync
{
    [ObservableProperty]
    private AppSettings settings;

    [ObservableProperty]
    private string? geocodioApiKey;

    [ObservableProperty]
    private string? placesBaseDirectory;

    public PlaceSettingsViewModel()
    {
        Settings = new AppSettings();
    }

    [RelayCommand]
    private async Task PickPlacesBaseFolder()
    {
        var result = await FolderPicker.Default.PickAsync();
        if (result.IsSuccessful)
        {
            PlacesBaseDirectory = result.Folder.Path;
        }
    }

    public async Task OnNavigatedToAsync()
    {
        Settings = SettingsManager.LoadSettings();
        GeocodioApiKey = Settings.GeocodioApiKey;
        PlacesBaseDirectory = Settings.PlacesBaseDirectory;
    }

    public Task OnNavigatedFromAsync()
    {
        Settings.GeocodioApiKey = GeocodioApiKey;
        Settings.PlacesBaseDirectory = PlacesBaseDirectory;
        SettingsManager.SaveSettings(Settings);
        return Task.CompletedTask;
    }
}