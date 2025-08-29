namespace SmartGenealogy.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private async void OnSettingsToolbarItemClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ThemeSettingsPage());
    }

    private async void AboutUs_Tapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AboutPage());
    }
}