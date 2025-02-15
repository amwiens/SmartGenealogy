using Injectio.Attributes;

namespace SmartGenealogy.Views;

[RegisterSingleton<SettingsPage>]
public partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();
    }
}