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
    }

    public Task OnNavigatedFromAsync()
    {
        SettingsManager.SaveSettings(Settings);
        return Task.CompletedTask;
    }
}