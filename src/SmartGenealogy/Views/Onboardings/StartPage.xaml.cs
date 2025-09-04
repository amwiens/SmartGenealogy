namespace SmartGenealogy.Views.Onboardings;

public partial class StartPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    public StartPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    private async void TakeTour_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WalkthroughPage(_serviceProvider));
    }

    private async void Skip_Clicked(object sender, EventArgs e)
    {
        Application.Current!.Windows[0].Page = new AppFlyout(_serviceProvider);
    }
}