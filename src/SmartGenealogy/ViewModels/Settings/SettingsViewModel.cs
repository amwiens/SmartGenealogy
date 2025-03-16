using CommunityToolkit.Mvvm.ComponentModel;

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