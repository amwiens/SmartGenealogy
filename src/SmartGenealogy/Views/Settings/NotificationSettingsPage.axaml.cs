using Injectio.Attributes;

using SmartGenealogy.Controls;

namespace SmartGenealogy.Views.Settings;

[RegisterSingleton<NotificationSettingsPage>]
public partial class NotificationSettingsPage : UserControlBase
{
    public NotificationSettingsPage()
    {
        InitializeComponent();
    }
}