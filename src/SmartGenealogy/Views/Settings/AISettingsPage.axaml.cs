using Injectio.Attributes;

using SmartGenealogy.Controls;

namespace SmartGenealogy.Views.Settings;

[RegisterSingleton<AISettingsPage>]
public partial class AISettingsPage : UserControlBase
{
    public AISettingsPage()
    {
        InitializeComponent();
    }
}