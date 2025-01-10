namespace SmartGenealogy.Views;

public partial class OllamaSettingsPage : BasePage
{
	public OllamaSettingsPage()
	{
		InitializeComponent();
		BindingContext = new OllamaSettingsViewModel();
	}
}