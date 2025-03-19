using SmartGenealogy.ViewModels;
using SmartGenealogy.Views.Base;

namespace SmartGenealogy.Views;

public partial class MainPage : BasePage
{
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}