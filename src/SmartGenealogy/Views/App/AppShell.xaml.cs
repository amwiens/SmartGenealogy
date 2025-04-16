namespace SmartGenealogy.Views.DemoApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		RegisterRoutes();
	}

	void RegisterRoutes()
	{
		Routing.RegisterRoute(nameof(ApplicationSettingsPage), typeof(ApplicationSettingsPage));
        Routing.RegisterRoute(nameof(AISettingsPage), typeof(AISettingsPage));
    }
}