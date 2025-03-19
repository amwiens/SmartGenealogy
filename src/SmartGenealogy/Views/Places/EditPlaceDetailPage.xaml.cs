using SmartGenealogy.ViewModels.Places;
using SmartGenealogy.Views.Base;

namespace SmartGenealogy.Views.Places;

public partial class EditPlaceDetailPage : BasePage
{
    public EditPlaceDetailPage(EditPlaceDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}