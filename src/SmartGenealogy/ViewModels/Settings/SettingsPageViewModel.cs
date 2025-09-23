namespace SmartGenealogy.ViewModels.Settings;

public partial class SettingsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _title = "Settings Page";

    [ObservableProperty]
    private bool _darkModeSwitchToggled;

    public SettingsPageViewModel()
    {
        DarkModeSwitchToggled = SmartGenealogySettings.UseDarkMode;
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