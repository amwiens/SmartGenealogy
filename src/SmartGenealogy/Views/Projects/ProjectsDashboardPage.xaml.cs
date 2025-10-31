namespace SmartGenealogy.Views.Projects;

/// <summary>
/// Projects dashboard page
/// </summary>
public partial class ProjectsDashboardPage : BasePage
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="viewModel">Projects dashboard page view model</param>
    public ProjectsDashboardPage(ProjectsDashboardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}