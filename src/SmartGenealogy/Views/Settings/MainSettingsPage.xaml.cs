namespace SmartGenealogy.Views;

public partial class MainSettingsPage : BasePage
{
    public MainSettingsPage()
    {
        InitializeComponent();
        BindingContext = new MainSettingsViewModel();
    }

    async void OllamaSettings_Tapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new OllamaSettingsPage());
    }
}