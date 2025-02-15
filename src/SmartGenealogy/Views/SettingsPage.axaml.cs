using Injectio.Attributes;

using SmartGenealogy.Controls;

namespace SmartGenealogy.Views;

[RegisterSingleton<SettingsPage>]
public partial class SettingsPage : UserControlBase
{
    public SettingsPage()
    {
        InitializeComponent();
    }
}