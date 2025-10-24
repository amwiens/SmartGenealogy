namespace SmartGenealogy.Views.Tools;

/// <summary>
/// Web links page.
/// </summary>
public partial class WebLinksPage : BasePage
{
    /// <summary>
    /// Constructor
    /// </summary>
    public WebLinksPage(WebLinksPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}