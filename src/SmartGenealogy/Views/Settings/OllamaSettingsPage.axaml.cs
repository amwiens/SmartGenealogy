using Injectio.Attributes;

using SmartGenealogy.Controls;

namespace SmartGenealogy.Views.Settings;

[RegisterSingleton<OllamaSettingsPage>]
public partial class OllamaSettingsPage : UserControlBase
{
    public OllamaSettingsPage()
    {
        InitializeComponent();
    }
}