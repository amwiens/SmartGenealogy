namespace SmartGenealogy.Views.Tools;

public partial class ToolsPage : ContentPage
{
	public ToolsPage(ToolsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}