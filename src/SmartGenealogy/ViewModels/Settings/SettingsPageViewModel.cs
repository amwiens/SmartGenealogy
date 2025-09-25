namespace SmartGenealogy.ViewModels.Settings;

public partial class SettingsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _title = "Settings Page";

    [ObservableProperty]
    private bool _darkModeSwitchToggled;

    [ObservableProperty]
    private bool _openLastDatabaseToggled;

    public SettingsPageViewModel()
    {
        DarkModeSwitchToggled = SmartGenealogySettings.UseDarkMode;
        OpenLastDatabaseToggled = SmartGenealogySettings.OpenLastDatabaseOnStartup;
    }

    [RelayCommand]
    private void ToggleOpenLastDatabase()
    {
        SmartGenealogySettings.OpenLastDatabaseOnStartup = OpenLastDatabaseToggled;
        SmartGenealogySettings.SaveSettings();
    }

    [RelayCommand]
    private void ToggleDarkMode()
    {
        if (DarkModeSwitchToggled)
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
}