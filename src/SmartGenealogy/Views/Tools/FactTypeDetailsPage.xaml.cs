namespace SmartGenealogy.Views.Tools;

public partial class FactTypeDetailsPage : BasePage
{
	public FactTypeDetailsPage(FactTypeDetailsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}