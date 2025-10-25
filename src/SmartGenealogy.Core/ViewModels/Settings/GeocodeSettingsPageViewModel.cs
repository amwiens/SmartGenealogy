namespace SmartGenealogy.Core.ViewModels.Settings;

/// <summary>
/// Geocode settings page view model.
/// </summary>
public partial class GeocodeSettingsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _geocodePlaceOnSave;

    [ObservableProperty]
    private string? _locationIQAPIKey;

    /// <summary>
    /// Constructor.
    /// </summary>
    public GeocodeSettingsPageViewModel()
    {
        GeocodePlaceOnSave = SmartGenealogySettings.GeocodePlaceOnSave;
        LocationIQAPIKey = SmartGenealogySettings.LocationIQAPIKey;
    }

    /// <summary>
    /// Handles the toggle of the Geocode Place on Save switch.
    /// </summary>
    [RelayCommand]
    private void ToggleGeocodePlaceOnSave()
    {
        SmartGenealogySettings.GeocodePlaceOnSave = GeocodePlaceOnSave;
        SmartGenealogySettings.SaveSettings();
    }

    /// <summary>
    /// Handles the text changed event of the LocationIQ API Key text box.
    /// </summary>
    [RelayCommand]
    private void LocationIQAPIKeyChanged()
    {
        SmartGenealogySettings.LocationIQAPIKey = LocationIQAPIKey;
        SmartGenealogySettings.SaveSettings();
    }
}