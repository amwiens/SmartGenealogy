using SmartGenealogy.Views.Places;

namespace SmartGenealogy
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddPlacePage), typeof(AddPlacePage));
            Routing.RegisterRoute(nameof(AddPlaceDetailPage), typeof(AddPlaceDetailPage));
            Routing.RegisterRoute(nameof(EditPlacePage), typeof(EditPlacePage));
            Routing.RegisterRoute(nameof(EditPlaceDetailPage), typeof(EditPlaceDetailPage));
            Routing.RegisterRoute(nameof(PlacePage), typeof(PlacePage));
            Routing.RegisterRoute(nameof(PlaceDetailPage), typeof(PlaceDetailPage));
        }
    }
}