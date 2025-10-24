namespace SmartGenealogy.Views.Tools;

/// <summary>
/// Web link page.
/// </summary>
public partial class WebLinkPage : BasePage
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public WebLinkPage(WebLinkPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}