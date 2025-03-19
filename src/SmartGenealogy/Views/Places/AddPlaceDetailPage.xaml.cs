using SmartGenealogy.ViewModels.Places;
using SmartGenealogy.Views.Base;

namespace SmartGenealogy.Views.Places;

public partial class AddPlaceDetailPage : BasePage
{
    public AddPlaceDetailPage(AddPlaceDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}