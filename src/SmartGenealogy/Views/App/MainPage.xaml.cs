namespace SmartGenealogy.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel viewModel;

    public MainPage()
    {
        InitializeComponent();
        this.viewModel = App.Current.Services.GetService<MainViewModel>();
        BindingContext = viewModel;
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

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await viewModel.OnNavigatedToAsync();
    }

    protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        await viewModel.OnNavigatedFromAsync();
    }
}