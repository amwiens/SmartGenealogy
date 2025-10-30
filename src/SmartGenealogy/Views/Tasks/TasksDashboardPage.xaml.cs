namespace SmartGenealogy.Views.Tasks;

/// <summary>
/// Tasks dashboard page
/// </summary>
public partial class TasksDashboardPage : BasePage
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="viewModel">Tasks dashboard page view model</param>
    public TasksDashboardPage(TasksDashboardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}