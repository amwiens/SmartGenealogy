using SmartGenealogy.ViewModels.Settings;

namespace SmartGenealogy.Views.Settings;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}