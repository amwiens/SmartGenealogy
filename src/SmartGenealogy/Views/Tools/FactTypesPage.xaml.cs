namespace SmartGenealogy.Views.Tools;

public partial class FactTypesPage : BasePage
{
	public FactTypesPage(FactTypesPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}