namespace SmartGenealogy.Views;

public partial class MainMenuPage : ContentPage
{
    public MainMenuPage(Action<Page> openPageAsRoot)
    {
        InitializeComponent();
        BindingContext = new MainMenuViewModel(Navigation, openPageAsRoot);
    }
}