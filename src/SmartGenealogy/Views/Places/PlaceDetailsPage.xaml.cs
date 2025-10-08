namespace SmartGenealogy.Views.Places;

/// <summary>
/// Place details page.
/// </summary>
public partial class PlaceDetailsPage : ContentPage
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="viewModel">Place details page view model.</param>
    public PlaceDetailsPage(PlaceDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}