using SmartGenealogy.ViewModels.Media;
using SmartGenealogy.Views.Base;

namespace SmartGenealogy.Views.Media;

public partial class AddMediaPage : BasePage
{
	public AddMediaPage(AddMediaViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}