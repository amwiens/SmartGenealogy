using SmartGenealogy.ViewModels.Places;

namespace SmartGenealogy.Views.Places;

public partial class PlaceDetailPage : ContentPage
{
    private readonly PlaceDetailViewModel viewModel;

    public PlaceDetailPage(PlaceDetailViewModel viewModel)
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