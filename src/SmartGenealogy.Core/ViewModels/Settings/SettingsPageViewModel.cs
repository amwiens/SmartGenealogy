namespace SmartGenealogy.Core.ViewModels.Settings;

/// <summary>
/// Settings page view model.
/// </summary>
public partial class SettingsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _openLastDatabase;

    [ObservableProperty]
    private bool _useDarkMode;

    /// <summary>
    /// Constructor.
    /// </summary>
    public SettingsPageViewModel()
    {
        OpenLastDatabase = SmartGenealogySettings.OpenLastDatabaseOnStartup;
        UseDarkMode = SmartGenealogySettings.UseDarkMode;
    }

    /// <summary>
    /// Handles the toggle of the Open Last Database switch.
    /// </summary>
    [RelayCommand]
    private void ToggleOpenLastDatabase()
    {
        SmartGenealogySettings.OpenLastDatabaseOnStartup = OpenLastDatabase;
        SmartGenealogySettings.SaveSettings();
    }

    /// <summary>
    /// Handles the toggle of the Use Dark Mode switch.
    /// </summary>
    [RelayCommand]
    private void ToggleUseDarkMode()
    {
        if (UseDarkMode)
        {
            Application.Current!.Resources.ApplyDarkTheme();
            SmartGenealogySettings.UseDarkMode = true;
        }
        else
        {
            Application.Current!.Resources.ApplyLightTheme();
            SmartGenealogySettings.UseDarkMode = false;
        }
        SmartGenealogySettings.SaveSettings();
    }

    /// <summary>
    /// Go to geocode settings page.
    /// </summary>
    [RelayCommand]
    private async Task GoToGeocodeSettings()
    {
        await Shell.Current.GoToAsync("geocodeSettings");
    }
}