using SmartGenealogy.Views.Media;
using SmartGenealogy.Views.Places;
using SmartGenealogy.Views.Settings;

namespace SmartGenealogy
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddMediaPage), typeof(AddMediaPage));
            Routing.RegisterRoute(nameof(AddPlacePage), typeof(AddPlacePage));
            Routing.RegisterRoute(nameof(AddPlaceDetailPage), typeof(AddPlaceDetailPage));
            Routing.RegisterRoute(nameof(EditPlacePage), typeof(EditPlacePage));
            Routing.RegisterRoute(nameof(EditPlaceDetailPage), typeof(EditPlaceDetailPage));
            Routing.RegisterRoute(nameof(PlacePage), typeof(PlacePage));
            Routing.RegisterRoute(nameof(PlaceDetailPage), typeof(PlaceDetailPage));
            Routing.RegisterRoute(nameof(ImageSettingsPage), typeof(ImageSettingsPage));
            Routing.RegisterRoute(nameof(PlaceSettingsPage), typeof(PlaceSettingsPage));
            Routing.RegisterRoute(nameof(AISettingsPage), typeof(AISettingsPage));
        }
    }
}