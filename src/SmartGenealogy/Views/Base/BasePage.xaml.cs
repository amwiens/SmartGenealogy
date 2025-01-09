namespace SmartGenealogy.Views;

public partial class BasePage : ContentPage
{
    public IList<Microsoft.Maui.IView> PageContent => PageContentGrid.Children;

    public BasePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
#if WINDOWS
        CheckIfRootPage();
#endif
    }

    public async void GoBack_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }

    protected void CheckIfRootPage()
    {
        if (Navigation.NavigationStack.Count == 1)
        {
            // Root page
            backNavigation.IsVisible = false;
        }
        else
        {
            backNavigation.IsVisible = true;
        }
    }

    public static BindableProperty BaseTitleProperty =
        BindableProperty.Create(
            nameof(BaseTitle),
            typeof(string),
            typeof(BasePage),
            string.Empty);

    public string BaseTitle
    {
        get => (string)GetValue(BaseTitleProperty);
        set => SetValue(BaseTitleProperty, value);
    }

    public static BindableProperty BaseTitleColorProperty =
        BindableProperty.Create(
            nameof(BaseTitleColor),
            typeof(Color),
            typeof(BasePage),
            defaultValue: Color.FromArgb("#000000"));

    public Color BaseTitleColor
    {
        get => (Color)GetValue(BaseTitleColorProperty);
        set => SetValue(BaseTitleColorProperty, value);
    }
}