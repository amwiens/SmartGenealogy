namespace SmartGenealogy.Views.Tools;

/// <summary>
/// Fact types page.
/// </summary>
public partial class FactTypesPage : BasePage
{
    /// <summary>
    /// Constructor
    /// </summary>
    public FactTypesPage(FactTypesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}