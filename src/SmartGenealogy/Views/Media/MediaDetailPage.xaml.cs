using SmartGenealogy.ViewModels.Media;

namespace SmartGenealogy.Views.Media;

public partial class MediaDetailPage : ContentPage
{
	private readonly MediaDetailViewModel viewModel;

    public MediaDetailPage(MediaDetailViewModel viewModel)
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