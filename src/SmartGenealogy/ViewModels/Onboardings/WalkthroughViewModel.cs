namespace SmartGenealogy.ViewModels.Onboardings;

public partial class WalkthroughViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private INavigation _navigationService;
    private Page _pageService;

    public WalkthroughViewModel(INavigation navigationService, Page pageService, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
        _pageService = pageService;

        Boardings = new ObservableCollection<Boarding>();
        CreateBoardingCollection();
    }

    private void CreateBoardingCollection()
    {
        Boardings = new ObservableCollection<Boarding>()
        {
            new Boarding
            {
                ImagePath = SmartGenealogyIcons.Table,
                Title = LocalizationResourceManager.Translate("StringWalkthroughDemoTitleStep1"),
                Subtitle = LocalizationResourceManager.Translate("StringWalkthroughDemoSubtitleStep1")
            },
            new Boarding
            {
                ImagePath = SmartGenealogyIcons.TableEdit,
                Title = LocalizationResourceManager.Translate("StringWalkthroughDemoTitleStep2"),
                Subtitle = LocalizationResourceManager.Translate("StringWalkthroughDemoSubtitleStep2")
            },
            new Boarding
            {
                ImagePath = SmartGenealogyIcons.CodeTagsCheck,
                Title = LocalizationResourceManager.Translate("StringWalkthroughDemoTitleStep3"),
                Subtitle = LocalizationResourceManager.Translate("StringWalkthroughDemoSubtitleStep3")
            }
        };
    }

    #region Commands

    [RelayCommand]
    private async Task Skip(object obj)
    {
        await CloseWalkThroughPage();
    }

    [RelayCommand]
    private async Task Next(object obj)
    {
        if (ValidateAndUpdatePosition())
        {
            await CloseWalkThroughPage();
        }
    }

    #endregion Commands

    #region Methods

    private bool ValidateAndUpdatePosition()
    {
        ValidateSelection(Position + 1);
        if (Position >= Boardings.Count - 1)
            return true;
        Position = Position + 1;
        return false;
    }

    private void ValidateSelection(int index)
    {
        if (index <= Boardings.Count - 2)
        {
            IsSkipButtonVisible = true;
            NextButtonText = AppTranslations.ButtonNext;
        }
        else
        {
            NextButtonText = AppTranslations.ButtonFinish;
            IsSkipButtonVisible = false;
        }
    }

    private async Task CloseWalkThroughPage()
    {
        Application.Current!.Windows[0].Page = new AppFlyout(_serviceProvider);
    }

    #endregion Methods

    #region Properties

    [ObservableProperty]
    public ObservableCollection<Boarding> _boardings;

    [ObservableProperty]
    private bool _isSkipButtonVisible = true;

    [ObservableProperty]
    private int _position;

    [ObservableProperty]
    private string _nextButtonText = AppTranslations.ButtonNext;

    #endregion Properties
}