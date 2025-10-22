namespace SmartGenealogy.Views.Places;

/// <summary>
/// Place page
/// </summary>
public partial class PlacePage : BasePage
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="viewModel">Place page view model</param>
    public PlacePage(PlacePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}