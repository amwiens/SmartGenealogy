namespace SmartGenealogy.Views;

public partial class SettingsPage : BasePage
{
    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = new SettingsViewModel();
    }
}