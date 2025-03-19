using SmartGenealogy.ViewModels.Places;
using SmartGenealogy.Views.Base;

namespace SmartGenealogy.Views.Places;

public partial class PlacesPage : BasePage
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