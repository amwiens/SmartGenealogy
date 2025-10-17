namespace SmartGenealogy.Views.Settings;

public partial class GeocodeSettingsPage : ContentPage
{
    public GeocodeSettingsPage(GeocodeSettingsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}