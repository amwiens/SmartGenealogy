namespace SmartGenealogy.Views.Sources;

/// <summary>
/// Source page
/// </summary>
public partial class SourcePage : BasePage
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="viewModel">Source page view model</param>
    public SourcePage(SourcePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}