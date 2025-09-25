namespace SmartGenealogy;

public partial class AppShell : Shell, IRecipient<DatabaseOpenMessage>
{
    private readonly DatabaseSettings _databaseSettings;
    private readonly AppShellViewModel _viewModel;

    public AppShell(AppShellViewModel viewModel, DatabaseSettings databaseSettings)
    {
        InitializeComponent();
        _databaseSettings = databaseSettings;
        _viewModel = viewModel;
        WeakReferenceMessenger.Default.Register(this);
        BindingContext = viewModel;
        AddFlyoutItems();
    }

    private void AddFlyoutItems()
    {
        Items.Clear();

        foreach (var item in _viewModel.MenuItems)
        {
            var flyoutItem = new FlyoutItem
            {
                Title = item.Title,
                Icon = item.FontImageSource,
                Route = item.Route
            };

            // You can customize which page to use based on Route or other properties
            ShellContent content = item.Route! switch
            {
                "main" => new ShellContent { ContentTemplate = new DataTemplate(typeof(MainPage)), Route = "main" },
                "media" => new ShellContent { ContentTemplate = new DataTemplate(typeof(MediaPage)), Route = "media" },
                "tools" => new ShellContent { ContentTemplate = new DataTemplate(typeof(ToolsPage)), Route = "tools" },
                "settings" => new ShellContent { ContentTemplate = new DataTemplate(typeof(SettingsPage)), Route = "settings" },
                _ => null
            };

            if (content != null && string.IsNullOrWhiteSpace(_databaseSettings.DatabaseName) && (item.Title == "Home" || item.Title == "Settings"))
                flyoutItem.Items.Add(content);
            else if (content != null && !string.IsNullOrEmpty(_databaseSettings.DatabaseName))
                flyoutItem.Items.Add(content);

            Items.Add(flyoutItem);
        }
    }

    public void Receive(DatabaseOpenMessage message)
    {
        AddFlyoutItems();
    }
}