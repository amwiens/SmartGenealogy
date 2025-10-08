namespace SmartGenealogy.Views.Places;

/// <summary>
/// Places page.
/// </summary>
public partial class PlacesPage : ContentPage
{
    /// <summary>
    /// Constructor
    /// </summary>
    public PlacesPage(PlacesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}