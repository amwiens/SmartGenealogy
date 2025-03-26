using SmartGenealogy.Views.Media;
using SmartGenealogy.Views.Places;
using SmartGenealogy.Views.Settings;

namespace SmartGenealogy.Views;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(AddMediaPage), typeof(AddMediaPage));
        Routing.RegisterRoute(nameof(MediaDetailPage), typeof(MediaDetailPage));
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

    protected override void OnAppearing()
    {
        base.OnAppearing();
#if WINDOWS
		CheckIfRootPage();
#endif
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);
        CheckIfRootPage();
        pageTitle.Text = Current.CurrentPage.Title;
    }
    public async void GoBack_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    protected void CheckIfRootPage()
    {
        if (Shell.Current.Navigation.NavigationStack.Count == 1)
        //if (Navigation.NavigationStack.Count == 1)
        {
            // Root page
            backNavigation.IsVisible = false;
        }
        else
        {
            backNavigation.IsVisible = true;
        }
    }
}