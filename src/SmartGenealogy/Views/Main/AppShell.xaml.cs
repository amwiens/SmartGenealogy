namespace SmartGenealogy.Views.Main;

/// <summary>
/// AppShell class.
/// </summary>
public partial class AppShell : Shell
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="viewModel">AppShell view model.</param>
    public AppShell(AppShellViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}