namespace SmartGenealogy.Mobile.Pages;

public partial class HomePage : ContentPage
{
	private HomeViewModel viewModel => BindingContext as HomeViewModel;

	public HomePage(HomeViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.InitializeAsync();
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}