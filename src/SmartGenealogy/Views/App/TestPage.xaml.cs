namespace SmartGenealogy.Views;

public partial class TestPage : ContentPage
{
    private readonly TestPageViewModel _viewModel;

    public TestPage()
    {
        InitializeComponent();
        _viewModel = new TestPageViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}