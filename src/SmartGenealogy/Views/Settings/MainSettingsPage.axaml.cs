using Injectio.Attributes;

using SmartGenealogy.Controls;

namespace SmartGenealogy.Views.Settings;

[RegisterSingleton<MainSettingsPage>]
public partial class MainSettingsPage : UserControlBase
{
    public MainSettingsPage()
    {
        InitializeComponent();
    }
}