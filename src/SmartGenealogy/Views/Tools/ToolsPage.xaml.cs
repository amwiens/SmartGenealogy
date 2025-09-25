namespace SmartGenealogy.Views.Tools;

public partial class ToolsPage : BasePage
{
	public ToolsPage(ToolsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}