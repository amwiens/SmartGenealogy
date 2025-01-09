namespace SmartGenealogy.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    public LocalizationResourceManager LocalizationResourceManager { get; }

    public SettingsViewModel()
    {
        LocalizationResourceManager = LocalizationResourceManager.Instance;

        LanguageSelected = AppSettings.SelectedLanguageItem;
        DarkModeSwitchToggled = AppSettings.IsDarkMode;
    }

    #region Properties

    public bool EnableDarkModeSwitch { get; set; } = true;

    private bool darkModeSwitchToggled = AppSettings.IsDarkMode;

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

    [ObservableProperty]
    private string? ollamaPath = AppSettings.OllamaPath;

    #endregion Properties

    #region Commands

    [RelayCommand]
    private async void LanguageItemTapped()
    {
        var popupViewModel = new LanguageSelectionPopupViewModel();
        var popup = new LanguageSelectionPopupPage { BindingContext = popupViewModel };
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

    partial void OnOllamaPathChanged(string? value)
    {
        AppSettings.OllamaPath = value!;
    }

    public void SetTheme()
    {
        if (EnableDarkModeSwitch)
        {
            if (darkModeSwitchToggled)
            {
                Application.Current!.Resources.ApplyDarkTheme();
                AppSettings.IsDarkMode = true;
            }
            else
            {
                Application.Current!.Resources.ApplyLightTheme();
                AppSettings.IsDarkMode = false;
            }
        }
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
}