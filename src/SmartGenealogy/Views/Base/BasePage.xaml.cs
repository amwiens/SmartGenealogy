namespace SmartGenealogy.Views.Base;

public partial class BasePage : ContentPage
{
    public IList<IView> PageContent => PageContentGrid.Children;

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
        await Shell.Current.GoToAsync("..");
    }

    protected void CheckIfRootPage()
    {
        var navStack = Shell.Current.Navigation.NavigationStack;
        if (navStack is null || navStack!.Count <= 1)
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
            string.Empty
        );

    public string BaseTitle
    {
        get => (string)this.GetValue(BaseTitleProperty);
        set => this.SetValue(BaseTitleProperty, value);
    }

    public static BindableProperty BaseTitleColorProperty =
        BindableProperty.Create(
            nameof(BaseTitleColor),
            typeof(Color),
            typeof(BasePage),
            defaultValue: Color.FromArgb("#000000")
        );

    public Color BaseTitleColor
    {
        get => (Color)this.GetValue(BaseTitleColorProperty);
        set => this.SetValue(BaseTitleColorProperty, value);
    }
}