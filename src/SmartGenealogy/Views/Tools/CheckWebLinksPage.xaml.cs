namespace SmartGenealogy.Views.Tools;

/// <summary>
/// Check web links page.
/// </summary>
public partial class CheckWebLinksPage : BasePage
{
    /// <summary>
    /// Constructor
    /// </summary>
    public CheckWebLinksPage(CheckWebLinksPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}