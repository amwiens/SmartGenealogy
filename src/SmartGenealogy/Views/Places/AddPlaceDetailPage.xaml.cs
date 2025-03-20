using SmartGenealogy.ViewModels.Places;

namespace SmartGenealogy.Views.Places;

public partial class AddPlaceDetailPage : ContentPage
{
    public AddPlaceDetailPage(AddPlaceDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}