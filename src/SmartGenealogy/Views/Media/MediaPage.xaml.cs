using SmartGenealogy.ViewModels.Media;

namespace SmartGenealogy.Views.Media;

public partial class MediaPage : ContentPage
{
	private readonly MediaViewModel viewModel;

	public MediaPage(MediaViewModel viewModel)
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