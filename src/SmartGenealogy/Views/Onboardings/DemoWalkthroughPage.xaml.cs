namespace SmartGenealogy.Views.Onboardings;

public partial class DemoWalkthroughPage : ContentPage
{
    public DemoWalkthroughPage()
    {
        InitializeComponent();
        BindingContext = new DemoWalkthroughViewModel(Navigation, this);
    }
}