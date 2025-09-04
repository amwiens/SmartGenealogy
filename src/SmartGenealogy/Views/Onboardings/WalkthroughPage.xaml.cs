namespace SmartGenealogy.Views.Onboardings;

public partial class WalkthroughPage : ContentPage
{
    public WalkthroughPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = new WalkthroughViewModel(Navigation, this, serviceProvider);
    }
}