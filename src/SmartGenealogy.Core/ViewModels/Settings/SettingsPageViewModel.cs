namespace SmartGenealogy.Core.ViewModels.Settings;

/// <summary>
/// Settings page view model.
/// </summary>
public partial class SettingsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _openLastDatabase;

    /// <summary>
    /// Constructor.
    /// </summary>
    public SettingsPageViewModel()
    {
        OpenLastDatabase = SmartGenealogySettings.OpenLastDatabaseOnStartup;
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
}