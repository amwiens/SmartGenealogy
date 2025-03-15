using SmartGenealogy.ViewModels.Places;

namespace SmartGenealogy.Views.Places;

public partial class EditPlacePage : ContentPage
{
	public EditPlacePage(EditPlaceViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}