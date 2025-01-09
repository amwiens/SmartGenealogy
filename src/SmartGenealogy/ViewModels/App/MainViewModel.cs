namespace SmartGenealogy.ViewModels;

public partial class MainViewModel : BaseViewModel, IRecipient<CultureChangeMessage>
{
    [ObservableProperty]
    private List<HomeBanner> _bannerItems;

    [ObservableProperty]
    private int _position;

    [ObservableProperty]
    private string text;

    [ObservableProperty]
    private string uri;

    [ObservableProperty]
    private string subject;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private bool isRTLLanguage;

    public ICommand ShareCommand { get; }
    public ICommand BuyNowCommand { get; }

    public MainViewModel()
    {
        LoadBannerCollection();
        WeakReferenceMessenger.Default.Register<CultureChangeMessage>(this);
        IsRTLLanguage = AppSettings.IsRTLLanguage;

        ShareCommand = new Command<View>(OnShare);
        BuyNowCommand = new Command<View>(OnBuyNow);
    }

    /// <summary>
    /// On received culture changed message, reload banner items
    /// </summary>
    /// <param name="message"></param>
    public void Receive(CultureChangeMessage message)
    {
        IsRTLLanguage = AppSettings.IsRTLLanguage;
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LoadBannerCollection();
        });
    }

    public void LoadBannerCollection()
    {
        BannerItems = new List<HomeBanner>
        {
        };
    }

    private async void OnShare(View element) =>
        await Share.RequestAsync(new ShareTextRequest
        {
            Subject = "Share Smart Genealogy app",
            Text = "Hi, checkout this awesome application",
            Uri = "",
            Title = "Share Smart Genealogy app",
            PresentationSourceBounds = element.GetAbsoluteBounds()
        });

    private async void OnBuyNow(View element)
    {
        var url = "";
        await Launcher.OpenAsync(url);
    }
}