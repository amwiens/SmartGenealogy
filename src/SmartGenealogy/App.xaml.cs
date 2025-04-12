#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif

#if IOS || MACCATALYST

using UIKit;

#endif

using LiveChartsCore;

using SmartGenealogy.Handlers;
using SmartGenealogy.ViewModels.Settings;

namespace SmartGenealogy;

public partial class App : Application
{
    public App()
    {
        Services = ConfigureServices();

        InitializeComponent();

        #region Handlers

        //Borderless entry
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
#if __ANDROID__
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif __IOS__ || __MACCATALYST__
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.Layer.BorderColor = UIColor.White.CGColor;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif __WINDOWS__
            handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
            handler.PlatformView.BorderThickness = new Windows.UI.Xaml.Thickness(0);
            handler.PlatformView.UnderlineThickness = new Windows.UI.Xaml.Thickness(0);
#endif
            }
        });

        //Borderless editor
        Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(nameof(BorderlessEditor), (handler, view) =>
        {
            if (view is BorderlessEditor)
            {
#if __ANDROID__
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif __IOS__ || __MACCATALYST__
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.Layer.BorderColor = UIColor.White.CGColor;
                //handler.PlatformView.BorderStyle = UIKit.UITextViewBorderStyle.None; //iOS 17+ only
#elif __WINDOWS__
            handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
            handler.PlatformView.BorderThickness = new Thickness(0);
#endif
            }
        });

        //Picker
        Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(BorderlessPicker), (handler, view) =>
        {
            if (view is BorderlessPicker)
            {
#if ANDROID
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif __IOS__ || __MACCATALYST__
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.Layer.BorderColor = UIColor.White.CGColor;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif __WINDOWS__
            handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
            handler.PlatformView.BorderThickness = new Thickness(0);
#endif
            }
        });

        #endregion Handlers

        if (AppSettings.IsFirstLaunching)
        {
            AppSettings.IsFirstLaunching = true; //Set to 'false' in production
            MainPage = new NavigationPage(new StartPage());
        }
        else
        {
            MainPage = GetMainPage();
        }
    }

    public new static App Current => (App)Application.Current;

    public IServiceProvider Services { get; }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddTransient<MainViewModel>();
        services.AddTransient<AppSettingsViewModel>();

        return services.BuildServiceProvider();
    }

    public static Page GetMainPage()
    {
        return new AppFlyout();
    }

    public void ChangeFlyoutDirection()
    {
        var flyoutPage = (AppFlyout)MainPage;
        if (AppSettings.IsRTLLanguage)
        {
            flyoutPage.Flyout.FlowDirection = FlowDirection.RightToLeft;
            flyoutPage.FlowDirection = FlowDirection.RightToLeft;
        }
        else
        {
            flyoutPage.Flyout.FlowDirection = FlowDirection.LeftToRight;
            flyoutPage.FlowDirection = FlowDirection.LeftToRight;
        }
    }

    protected override void OnStart()
    {
        base.OnStart();

        var appLanguageCode = string.Empty;
        if (AppSettings.SelectedLanguageItem == null)
        {
            var systemLanguageCode = CultureInfo.CurrentUICulture.Name;
            if (AppSettings.Languages.Any(x => x.Code.Equals(systemLanguageCode, StringComparison.OrdinalIgnoreCase)))
                appLanguageCode = systemLanguageCode;
            else
                appLanguageCode = AppSettings.DefaultLanguageCode;
        }
        else
            appLanguageCode = AppSettings.LanguageCodeSelected;

        LocalizationResourceManager.Instance.SetCulture(new CultureInfo(appLanguageCode));

        if (AppSettings.IsRTLLanguage || CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
        {
            FlowDirectionManager.Instance.FlowDirection = FlowDirection.RightToLeft;
        }
        else
        {
            FlowDirectionManager.Instance.FlowDirection = FlowDirection.LeftToRight;
        }

        AppTheme currentTheme = Application.Current.RequestedTheme;

        if (AppSettings.IsUseSystemTheme)
        {
            if (currentTheme == AppTheme.Dark)
                Application.Current.Resources.ApplyDarkTheme();
            else
                Application.Current.Resources.ApplyLightTheme();
        }
        else
        {
            if (AppSettings.IsDarkMode)
                Application.Current.Resources.ApplyDarkTheme();
            else
                Application.Current.Resources.ApplyLightTheme();
        }

        ThemeUtil.ApplyColorSet(AppSettings.SelectedPrimaryColorIndex);

        LiveCharts.Configure(config =>
            config
                // you can override the theme
                //.AddDarkTheme()

                // In case you need a non-Latin based font, you must register a typeface for SkiaSharp
                //.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('汉')) // <- Chinese
                //.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('あ')) // <- Japanese
                //.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('헬')) // <- Korean
                //.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('Ж'))  // <- Russian

                //.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('أ'))  // <- Arabic
                //.UseRightToLeftSettings() // Enables right to left tooltips

                // finally register your own mappers
                // you can learn more about mappers at:
                // https://livecharts.dev/docs/maui/2.0.0-rc2/Overview.Mappers

                // here we use the index as X, and the population as Y
                .HasMap<City>((city, index) => new(index, city.Population))
        // .HasMap<Foo>( .... )
        // .HasMap<Bar>( .... )
        );
    }

    public record City(string Name, double Population);
}