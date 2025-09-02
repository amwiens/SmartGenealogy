namespace SmartGenealogy;

public partial class MainMenuPage : ContentPage
{
    public MainMenuPage(Action<Page> openPageAsRoot, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = new MainMenuViewModel(Navigation, openPageAsRoot, serviceProvider);
    }
}