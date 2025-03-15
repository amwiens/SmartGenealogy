using SmartGenealogy.ViewModels.Places;

namespace SmartGenealogy.Views.Places;

public partial class EditPlaceDetailPage : ContentPage
{
    public EditPlaceDetailPage(EditPlaceDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}