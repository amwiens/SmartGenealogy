namespace SmartGenealogy.Views;

public partial class AppShell : Shell
{
	public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();

	public AppShell()
	{
		InitializeComponent();
		RegisterRoutes();
	}

	void RegisterRoutes()
	{
		Routes.Add(nameof(ThemeSettingsPage), typeof(ThemeSettingsPage));

		foreach (var route in Routes)
		{
			Routing.RegisterRoute(route.Key, route.Value);
        }
    }
}