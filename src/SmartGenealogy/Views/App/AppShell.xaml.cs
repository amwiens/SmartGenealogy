namespace SmartGenealogy.Views.DemoApp;

public partial class AppShell : Shell, IRecipient<DatabaseChangeMessage>
{
    public string DatabasePath { get; set; }

    public AppShell()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<DatabaseChangeMessage>(this);
        RegisterRoutes();
    }

    private void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(ApplicationSettingsPage), typeof(ApplicationSettingsPage));
        Routing.RegisterRoute(nameof(AISettingsPage), typeof(AISettingsPage));
    }

    public void Receive(DatabaseChangeMessage message)
    {
        // Handle the database change message here
        // For example, you can update the UI or perform any necessary actions
        if (!string.IsNullOrEmpty(message.Value))
        {
            menuPlaces.IsVisible = true;
        }
        else
        {
            menuPlaces.IsVisible = false;
        }
    }
}