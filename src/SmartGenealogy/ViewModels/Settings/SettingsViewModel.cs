using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Helpers;
using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Settings;

public partial class SettingsViewModel : ObservableObject, INavigationAwareAsync
{
    [ObservableProperty]
    private AppSettings settings;

    [ObservableProperty]
    private bool darkMode;

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

    partial void OnDarkModeChanged(bool value)
    {
        Settings.DarkMode = value;
        if (value)
            Application.Current!.Resources.ApplyDarkTheme();
        else
            Application.Current!.Resources.ApplyLightTheme();
    }

    public async Task OnNavigatedToAsync()
    {
        Settings = SettingsManager.LoadSettings();
        DarkMode = Settings.DarkMode;

    }

    public Task OnNavigatedFromAsync()
    {
        SettingsManager.SaveSettings(Settings);
        return Task.CompletedTask;
    }
}