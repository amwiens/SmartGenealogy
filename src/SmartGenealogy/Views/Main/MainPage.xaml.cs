namespace SmartGenealogy.Views.Main;

/// <summary>
/// MainPage class.
/// </summary>
public partial class MainPage : BasePage
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