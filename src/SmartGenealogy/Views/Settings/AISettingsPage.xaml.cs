using SmartGenealogy.ViewModels.Settings;
using SmartGenealogy.Views.Base;

namespace SmartGenealogy.Views.Settings;

public partial class AISettingsPage : BasePage
{
    private readonly AISettingsViewModel viewModel;

	public AISettingsPage(AISettingsViewModel viewModel)
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