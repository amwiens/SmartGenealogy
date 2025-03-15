using SmartGenealogy.ViewModels.Places;

namespace SmartGenealogy.Views.Places;

public partial class PlacePage : ContentPage
{
	private readonly PlaceViewModel viewModel;

	public PlacePage(PlaceViewModel viewModel)
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