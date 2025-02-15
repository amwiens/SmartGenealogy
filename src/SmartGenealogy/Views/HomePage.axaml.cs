using Avalonia.Controls;

using Injectio.Attributes;

namespace SmartGenealogy.Views;

[RegisterSingleton<HomePage>]
public partial class HomePage : UserControl
{
    public HomePage()
    {
        InitializeComponent();
    }
}