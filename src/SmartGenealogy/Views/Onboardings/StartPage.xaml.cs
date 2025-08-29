namespace SmartGenealogy.Views.Onboardings;

public partial class StartPage : ContentPage
{
    public StartPage()
    {
        InitializeComponent();
    }

    private async void TakeTour_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WalkthroughPage());
    }

    private async void Skip_Clicked(object sender, EventArgs e)
    {
        Application.Current!.Windows[0].Page = new AppFlyout();
    }
}