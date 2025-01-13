namespace SmartGenealogy.ViewModels;

public partial class MainMenuViewModel : ObservableObject, IRecipient<CultureChangeMessage>
{
    private INavigation _navigation;

    private Action<Page> _openPageAsRoot;

    [ObservableProperty]
    private List<MenuEntry> _mainMenuEntries;

    private bool _isGridView;

    private MenuEntry _selectedMainMenuEntry;

    public MainMenuViewModel(INavigation navigation, Action<Page> openPageAsRoot)
    {
        _navigation = navigation;
        _openPageAsRoot = openPageAsRoot;

        IsGridMenuSwitchToggled = AppSettings.IsMenuGridStyle;

        WeakReferenceMessenger.Default.Register<CultureChangeMessage>(this);

        LoadMenuData();
        var firstEntry = MainMenuEntries[0];
        MainMenuSelectedItem = firstEntry;

        MenuItemSelectionCommand = new Command<MenuEntry>(SelectedMenuItem);
    }

    public void Receive(CultureChangeMessage message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LoadMenuData();
        });
    }

    private void LoadMenuData()
    {
        MainMenuEntries = new List<MenuEntry>
        {
            new MenuEntry
            {
                Title = LocalizationResourceManager.Translate("MenuHome"),
                Icon = MaterialDesignIcons.Home,
                TargetType = typeof(MainPage)
            },
            new MenuEntry
            {
                Title = LocalizationResourceManager.Translate("MenuArticles"),
                Icon = MaterialDesignIcons.Note,
                TargetType = typeof(MainArticlesPage)
            },
            new MenuEntry
            {
                Title = LocalizationResourceManager.Translate("MenuSettings"),
                Icon = MaterialDesignIcons.Settings,
                TargetType = typeof(MainSettingsPage)
            },
        };
    }

    //public List<MenuEntry> MainMenuEntries
    //{
    //    get { return _mainMenuEntries; }
    //    set { SetProperty(ref _mainMenuEntries, value); }
    //}

    public bool IsGridMenuSwitchToggled
    {
        get { return _isGridView; }
        set { SetProperty(ref _isGridView, value); }
    }

    public MenuEntry MainMenuSelectedItem
    {
        get { return _selectedMainMenuEntry; }
        set
        {
            if (SetProperty(ref _selectedMainMenuEntry, value) && value != null)
            {
                NavigationPage navigationPage = new NavigationPage((Page)Activator.CreateInstance(value.TargetType));

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
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type TargetType { get; set; }
    }
}