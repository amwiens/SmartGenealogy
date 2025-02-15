using Injectio.Attributes;

using SmartGenealogy.Controls;

namespace SmartGenealogy.Views;

[RegisterSingleton<HomePage>]
public partial class HomePage : UserControlBase
{
    public HomePage()
    {
        InitializeComponent();
    }
}