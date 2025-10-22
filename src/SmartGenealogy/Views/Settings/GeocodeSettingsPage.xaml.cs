namespace SmartGenealogy.Views.Settings;

/// <summary>
/// Geocode settings page.
/// </summary>
public partial class GeocodeSettingsPage : BasePage
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public GeocodeSettingsPage(GeocodeSettingsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}