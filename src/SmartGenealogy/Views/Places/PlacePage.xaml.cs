using SmartGenealogy.ViewModels.Places;
using SmartGenealogy.Views.Base;

namespace SmartGenealogy.Views.Places;

public partial class PlacePage : BasePage
{
    private readonly PlaceViewModel viewModel;

    public PlacePage(PlaceViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;

        var map = new Mapsui.Map();
        mapControl.Map = map;
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