using SmartGenealogy.ViewModels.Media;

namespace SmartGenealogy.Views.Media;

public partial class AddMediaPage : ContentPage
{
	public AddMediaPage(AddMediaViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}