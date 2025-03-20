using SmartGenealogy.ViewModels.Places;

namespace SmartGenealogy.Views.Places;

public partial class AddPlacePage : ContentPage
{
    public AddPlacePage(AddPlaceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}