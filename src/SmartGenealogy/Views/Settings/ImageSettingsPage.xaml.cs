using SmartGenealogy.ViewModels.Settings;
using SmartGenealogy.Views.Base;

namespace SmartGenealogy.Views.Settings;

public partial class ImageSettingsPage : BasePage
{
    private readonly ImageSettingsViewModel viewModel;

	public ImageSettingsPage(ImageSettingsViewModel viewModel)
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