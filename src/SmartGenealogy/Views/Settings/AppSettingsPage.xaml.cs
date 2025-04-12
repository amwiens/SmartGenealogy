using SmartGenealogy.ViewModels.Settings;

namespace SmartGenealogy.Views.Settings;

public partial class AppSettingsPage : ContentPage
{
    private readonly AppSettingsViewModel viewModel;

    public AppSettingsPage()
	{
		InitializeComponent();
		this.viewModel = App.Current.Services.GetService<AppSettingsViewModel>();
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