namespace SmartGenealogy.ViewModels;

public partial class MainMenuViewModel : ObservableObject, IRecipient<CultureChangeMessage>, IRecipient<DatabaseChangeMessage>
{
    private readonly DatabaseSettings _databaseSettings;
    private INavigation _navigation;

    private readonly IServiceProvider _serviceProvider;

    private Action<Page> _openPageAsRoot;

    private List<MenuEntry>? _mainMenuEntries;

    private bool _isGridView;

    private bool _databaseOpen;

    private MenuEntry? _selectedMainMenuEntry;

    [ObservableProperty]
    private string? _databaseName;

    public MainMenuViewModel(INavigation navigation, Action<Page> openPageAsRoot, IServiceProvider serviceProvider, DatabaseSettings databaseSettings)
    {
        _navigation = navigation;
        _openPageAsRoot = openPageAsRoot;
        _serviceProvider = serviceProvider;
        _databaseSettings = databaseSettings;

        IsGridMenuSwitchToggled = AppSettings.IsMenuGridStyle;

        WeakReferenceMessenger.Default.Register<CultureChangeMessage>(this);
        WeakReferenceMessenger.Default.Register<DatabaseChangeMessage>(this);
        WeakReferenceMessenger.Default.Register<MainMenuGridStyleMessage>(this, (recipient, message) =>
        {
            IsGridMenuSwitchToggled = (bool)message.Value;
        });

        LoadMenuData();

        var firstEntry = MainMenuEntries[0];
        MainMenuSelectedItem = firstEntry;

        MenuItemSelectionCommand = new Command<MenuEntry>(SelectedMenuItem);
    }

    /// <summary>
    /// On received culture changed message, reload Menu item
    /// </summary>
    /// <param name="message"></param>
    public void Receive(CultureChangeMessage message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LoadMenuData();
        });
    }


    public void Receive(DatabaseChangeMessage message)
    {
        DatabaseName = _databaseSettings.DatabaseName;
        MainThread.BeginInvokeOnMainThread(() =>
        {
            _databaseOpen = (bool)message.Value;
            LoadMenuData();
        });
    }

    private void LoadMenuData()
    {
        MainMenuEntries = 
        [
            new MenuEntry
            {
                Title = LocalizationResourceManager.Translate("MenuHome"),
                Icon = MaterialDesignIcons.Home,
                TargetType = typeof(MainPage)
            },
        ];

        if (_databaseOpen)
        {
            MainMenuEntries.Add(new MenuEntry
            {
                Title = LocalizationResourceManager.Translate("MenuPeople"),
                Icon = MaterialDesignIcons.Person,
                TargetType = typeof(PeoplePage)
            });
        }
        MainMenuEntries.Add(new MenuEntry
        {
            Title = LocalizationResourceManager.Translate("MenuControls"),
            Icon = MaterialDesignIcons.ViewCompact,
            TargetType = typeof(ControlsOverviewPage)
        });
        MainMenuEntries.Add(new MenuEntry()
        {
            Title = LocalizationResourceManager.Translate("MenuIcon"),
            Icon = MaterialDesignIcons.InsertEmoticon,
            TargetType = typeof(FontIconsPage)
        });
        MainMenuEntries.Add(new MenuEntry()
        {
            Title = LocalizationResourceManager.Translate("MenuAbout"),
            Icon = MaterialDesignIcons.Info,
            TargetType = typeof(AboutPage)
        });
        MainMenuEntries.Add(new MenuEntry()
        {
            Title = LocalizationResourceManager.Translate("MenuPrivacy"),
            Icon = MaterialDesignIcons.Security,
            TargetType = typeof(PrivacyPolicyPage)
        });
    }

    public List<MenuEntry> MainMenuEntries
    {
        get { return _mainMenuEntries!; }
        set { SetProperty(ref _mainMenuEntries, value); }
    }

    public bool IsGridMenuSwitchToggled
    {
        get { return _isGridView; }
        set { SetProperty(ref _isGridView, value); }
    }

    public MenuEntry MainMenuSelectedItem
    {
        get { return _selectedMainMenuEntry!; }
        set
        {
            if (SetProperty(ref _selectedMainMenuEntry, value) && value != null)
            {
                //NavigationPage navigationPage = new((Page)Activator.CreateInstance(value.TargetType!)!);
                NavigationPage navigationPage = new((Page)_serviceProvider.GetRequiredService(value.TargetType!)!);

                _openPageAsRoot(navigationPage);

                _selectedMainMenuEntry = null;
                OnPropertyChanged(nameof(MainMenuSelectedItem));
            }
        }
    }

    public ICommand MenuItemSelectionCommand { get; private set; }

    private async void SelectedMenuItem(MenuEntry obj)
    {
        MainMenuSelectedItem = obj;
    }

    public class MenuEntry
    {
        public string? Title { get; set; }
        public string? Icon { get; set; }
        public Type? TargetType { get; set; }
    }
}