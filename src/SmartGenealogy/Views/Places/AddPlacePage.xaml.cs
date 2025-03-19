using SmartGenealogy.ViewModels.Places;
using SmartGenealogy.Views.Base;

namespace SmartGenealogy.Views.Places;

public partial class AddPlacePage : BasePage
{
    public AddPlacePage(AddPlaceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}