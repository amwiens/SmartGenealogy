using SmartGenealogy.ViewModels.Places;
using SmartGenealogy.Views.Base;

namespace SmartGenealogy.Views.Places;

public partial class EditPlacePage : BasePage
{
    public EditPlacePage(EditPlaceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}