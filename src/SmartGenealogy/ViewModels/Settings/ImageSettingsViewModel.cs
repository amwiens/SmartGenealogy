using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SmartGenealogy.Models;
using SmartGenealogy.Services;

namespace SmartGenealogy.ViewModels.Settings;

public partial class ImageSettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private AppSettings settings;

    [ObservableProperty]
    private string? tesseractLanguageFileLocation;

    public ImageSettingsViewModel()
    {
        Settings = new AppSettings();
    }

    [RelayCommand]
    private async Task PickTesseractFolder()
    {
        var result = await FolderPicker.Default.PickAsync();
        if (result.IsSuccessful)
        {
            TesseractLanguageFileLocation = result.Folder.Path;
        }
    }

    public async Task OnNavigatedToAsync()
    {
        Settings = SettingsManager.LoadSettings();
        TesseractLanguageFileLocation = Settings.TesseractLanguageFileLocation;
    }

    public Task OnNavigatedFromAsync()
    {
        Settings.TesseractLanguageFileLocation = TesseractLanguageFileLocation;
        SettingsManager.SaveSettings(Settings);
        return Task.CompletedTask;
    }
}