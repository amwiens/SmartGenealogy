using CommunityToolkit.Mvvm.ComponentModel;

using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Settings;

public partial class AISettingsViewModel : ObservableObject, INavigationAwareAsync
{
    private readonly OllamaService _ollamaService;

    [ObservableProperty]
    private AppSettings settings;

    public AISettingsViewModel(OllamaService ollamaService)
    {
        _ollamaService = ollamaService;

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