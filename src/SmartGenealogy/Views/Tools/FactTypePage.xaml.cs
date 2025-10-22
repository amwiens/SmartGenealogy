namespace SmartGenealogy.Views.Tools;

/// <summary>
/// Fact type page.
/// </summary>
public partial class FactTypePage : BasePage
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public FactTypePage(FactTypePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}