using SmartGenealogy.ViewModels.Places;

namespace SmartGenealogy.Views.Places;

public partial class PlacesPage : ContentPage
{
	private readonly PlacesViewModel viewModel;

	public PlacesPage(PlacesViewModel viewModel)
	{
		InitializeComponent();
		this.viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await viewModel.OnNavigatedToAsync();
    }

    protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        await viewModel.OnNavigatedFromAsync();
    }
}