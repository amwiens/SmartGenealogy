using Injectio.Attributes;

using SmartGenealogy.Controls;

namespace SmartGenealogy.Views;

[RegisterSingleton<PlacesPage>]
public partial class PlacesPage : UserControlBase
{
    public PlacesPage()
    {
        InitializeComponent();
    }
}