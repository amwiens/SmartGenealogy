namespace SmartGenealogy.Views.Settings;

/// <summary>
/// Settings page.
/// </summary>
public partial class SettingsPage : BasePage
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="viewModel">Settings page view model.</param>
    public SettingsPage(SettingsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}