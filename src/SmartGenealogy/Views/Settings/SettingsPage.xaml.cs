namespace SmartGenealogy.Views.Settings;

public partial class SettingsPage : BasePage
{
	public SettingsPage(SettingsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}