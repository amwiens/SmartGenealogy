namespace SmartGenealogy;

public partial class AppFlyout : FlyoutPage
{
    public AppFlyout(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        var databaseSettings = serviceProvider.GetRequiredService<DatabaseSettings>();
        Flyout = new MainMenuPage(LaunchDetailPage, serviceProvider, databaseSettings);
    }

    private void LaunchDetailPage(Page page)
    {
        Detail = page;
        if (!((IFlyoutPageController)this).ShouldShowSplitMode)
            IsPresented = false;
    }
}