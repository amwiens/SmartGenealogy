namespace SmartGenealogy;

public partial class AppFlyout : FlyoutPage
{
    public AppFlyout(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        Flyout = new MainMenuPage(LaunchDetailPage, serviceProvider);
    }

    private void LaunchDetailPage(Page page)
    {
        Detail = page;
        if (!((IFlyoutPageController)this).ShouldShowSplitMode)
            IsPresented = false;
    }
}