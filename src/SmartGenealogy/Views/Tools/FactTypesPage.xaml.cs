namespace SmartGenealogy.Views.Tools;

/// <summary>
/// Fact types page.
/// </summary>
public partial class FactTypesPage : ContentPage
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