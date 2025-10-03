namespace SmartGenealogy.Views;

/// <summary>
/// MainPage class.
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="viewModel">Main page view model.</param>
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}