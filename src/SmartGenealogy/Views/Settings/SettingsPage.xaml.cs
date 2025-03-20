using SmartGenealogy.ViewModels.Settings;

namespace SmartGenealogy.Views.Settings;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel viewModel;

	public SettingsPage(SettingsViewModel viewModel)
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