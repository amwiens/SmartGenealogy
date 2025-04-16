namespace SmartGenealogy.ViewModels.Settings;

public partial class ApplicationSettingsViewModel : ObservableObject
{
    public LocalizationResourceManager LocalizationResourceManager { get; }

    public ApplicationSettingsViewModel()
    {
        LocalizationResourceManager = LocalizationResourceManager.Instance;
        CreatePrimaryColorCollection();

        LanguageSelected = AppSettings.SelectedLanguageItem;
        DarkModeSwitchToggled = AppSettings.IsDarkMode;
        UseSystemThemeSwitchToggled = AppSettings.IsUseSystemTheme;
        SelectedPrimaryColorItem = AppSettings.SelectedPrimaryColorCollectionItem;
    }

    #region Properties

    private List<PrimaryColorItem> primaryColorItems;

    public List<PrimaryColorItem> PrimaryColorItems
    {
        get => primaryColorItems;
        set
        {
            SetProperty(ref primaryColorItems, value);
        }
    }

    private PrimaryColorItem selectedPrimaryColorItem;

    public PrimaryColorItem SelectedPrimaryColorItem
    {
        get => selectedPrimaryColorItem;
        set
        {
            SetProperty(ref selectedPrimaryColorItem, value);
            AppSettings.SelectedPrimaryColorCollectionItem = value;
            AppSettings.SelectedPrimaryColorIndex = value.Index;
            ThemeUtil.ApplyColorSet(value.Index);
        }
    }

    private int selectedPrimaryColor;

    public int SelectedPrimaryColor
    {
        get => selectedPrimaryColor;
        set
        {
            SetProperty(ref selectedPrimaryColor, value);
            AppSettings.SelectedPrimaryColorIndex = value;
        }
    }

    private bool useSystemThemeSwitchToggled;

    public bool UseSystemThemeSwitchToggled
    {
        get => useSystemThemeSwitchToggled;
        set
        {
            SetProperty(ref useSystemThemeSwitchToggled, value);
            AppSettings.IsUseSystemTheme = value;
            EnableDarkModeSwitch = !value;
            SetTheme();
        }
    }

    public bool EnableDarkModeSwitch { get; set; } = true;

    private bool darkModeSwitchToggled;

    public bool DarkModeSwitchToggled
    {
        get => darkModeSwitchToggled;
        set
        {
            SetProperty(ref darkModeSwitchToggled, value);
            SetTheme();
        }
    }

    [ObservableProperty]
    private LanguageSelectItem languageSelected;

    //public LanguageSelectItem LanguageSelected
    //{
    //    get => languageSelected;
    //    set
    //    {
    //        SetProperty(ref languageSelected, value);
    //    }
    //}

    #endregion Properties

    #region Commands

    [RelayCommand]
    private async void LanguageItemTapped()
    {
        var popupViewModel = new LanguageSelectionPopupViewModel();
        var popup = new LanguageSelectionPopupPage { BindingContext = popupViewModel };
        //await PopupNavigation.Instance.PushAsync(popup);
        await PopupAction.DisplayPopup(popup);

        // Await the result from the popup
        var result = await popupViewModel.PopupClosedTask;
        if (result != null)
        {
            LanguageSelected = result;

            SetLanguage(result);
            WeakReferenceMessenger.Default.Send(new CultureChangeMessage(result.Code));
        }
    }

    #endregion Commands

    #region Methods

    public void CreatePrimaryColorCollection()
    {
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption1", out var primaryColorOption1);
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption2", out var primaryColorOption2);
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption3", out var primaryColorOption3);
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption4", out var primaryColorOption4);
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption5", out var primaryColorOption5);

        PrimaryColorItems = new List<PrimaryColorItem>
        {
            new PrimaryColorItem()
            {
                Index = 0,
                Color = (Color)primaryColorOption1
            },
            new PrimaryColorItem()
            {
                Index = 1,
                Color = (Color)primaryColorOption2
            },
            new PrimaryColorItem()
            {
                Index = 2,
                Color = (Color)primaryColorOption3
            },
            new PrimaryColorItem()
            {
                Index = 3,
                Color = (Color)primaryColorOption4
            },
            new PrimaryColorItem()
            {
                Index = 4,
                Color = (Color)primaryColorOption5
            }
        };
    }

    public void SetTheme()
    {
        if (EnableDarkModeSwitch)
        {
            if (darkModeSwitchToggled)
            {
                Application.Current.Resources.ApplyDarkTheme();
                AppSettings.IsDarkMode = true;
            }
            else
            {
                Application.Current.Resources.ApplyLightTheme();
                AppSettings.IsDarkMode = false;
            }
        }
        else if (useSystemThemeSwitchToggled)
        {
            AppTheme currentTheme = Application.Current.RequestedTheme;

            if (currentTheme == AppTheme.Dark)
                Application.Current.Resources.ApplyDarkTheme();
            else
                Application.Current.Resources.ApplyLightTheme();
        }

        ThemeUtil.ApplyColorSet(AppSettings.SelectedPrimaryColorIndex);
    }

    public void SetLanguage(LanguageSelectItem languageSelectedItem)
    {
        CultureInfo ci = new CultureInfo(languageSelectedItem.Code);
        LocalizationResourceManager.Instance.SetCulture(ci);

        AppSettings.SelectedLanguageItem = languageSelectedItem;
        AppSettings.LanguageCodeSelected = languageSelectedItem.Code;

        if (languageSelectedItem.IsRTL)
        {
            FlowDirectionManager.Instance.FlowDirection = FlowDirection.RightToLeft;
            AppSettings.IsRTLLanguage = true;
        }
        else
        {
            FlowDirectionManager.Instance.FlowDirection = FlowDirection.LeftToRight;
            AppSettings.IsRTLLanguage = false;
        }

        (Application.Current as App).ChangeFlyoutDirection();
    }

    #endregion Methods

    public async Task OnNavigatedToAsync()
    {
        await Task.CompletedTask;
    }

    public async Task OnNavigatedFromAsync()
    {
        await Task.CompletedTask;
    }
}