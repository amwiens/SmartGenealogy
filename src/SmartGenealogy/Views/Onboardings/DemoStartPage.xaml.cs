using SmartGenealogy.Views.DemoApp;

namespace SmartGenealogy.Views.Onboardings;

public partial class DemoStartPage : ContentPage
{
    public DemoStartPage()
    {
        InitializeComponent();
    }

    private async void TakeTour_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DemoWalkthroughPage());
    }

    private async void Skip_Clicked(object sender, EventArgs e)
    {
        //Application.Current.MainPage = new AppFlyout();
        Application.Current.Windows[0].Page = new AppShell();
    }
}