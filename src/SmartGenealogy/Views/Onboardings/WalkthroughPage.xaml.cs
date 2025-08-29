namespace SmartGenealogy.Views.Onboardings;

public partial class WalkthroughPage : ContentPage
{
    public WalkthroughPage()
    {
        InitializeComponent();
        BindingContext = new WalkthroughViewModel(Navigation, this);
    }
}