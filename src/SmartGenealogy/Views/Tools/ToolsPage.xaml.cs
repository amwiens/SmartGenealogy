namespace SmartGenealogy.Views.Tools;

/// <summary>
/// Tools page.
/// </summary>
public partial class ToolsPage : ContentPage
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="viewModel">Tools page view model</param>
    public ToolsPage(ToolsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}