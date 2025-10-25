namespace SmartGenealogy.Views.Sources;

/// <summary>
/// Sources page.
/// </summary>
public partial class SourcesPage : BasePage
{
    /// <summary>
    /// Constructor
    /// </summary>
    public SourcesPage(SourcesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}