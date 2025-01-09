namespace SmartGenealogy.Views;

public partial class AppFlyout : FlyoutPage
{
	public AppFlyout()
	{
		InitializeComponent();

		Flyout = new MainMenuPage(LaunchDetailPage);
	}

	private void LaunchDetailPage(Page page)
	{
		Detail = page;
		if (!((IFlyoutPageController)this).ShouldShowSplitMode)
			IsPresented = false;
	}
}