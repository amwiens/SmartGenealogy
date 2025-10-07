namespace SmartGenealogy.Views.Tools;

public partial class FactTypePage : ContentPage
{
    public FactTypePage(FactTypePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}