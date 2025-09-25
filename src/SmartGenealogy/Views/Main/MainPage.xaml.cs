namespace SmartGenealogy.Views.Main;

public partial class MainPage : BasePage
{
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}