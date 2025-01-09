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

using SmartGenealogy.Handlers;

namespace SmartGenealogy;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        #region Handlers

        // Borderless entry
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

        // Borderless editor
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
                //handler.PlatformView.BorderStyle = UIKit.UITextViewBorderStyle.None; // iOS 17+ only
#elif __WINDOWS__
                handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
                handler.PlatformView.BorderThickness = new Thickness(0);
#endif
            }
        });

        // Picker
        Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(BorderlessPicker), (handler, view) =>
        {
            if (view is BorderlessPicker)
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
                handler.PlatformView.BorderThickness = new Thickness(0);
#endif
            }
        });

        #endregion

        //if (AppSettings.IsFirstLaunching)
        //{
        //    AppSettings.IsFirstLaunching = false; // Set to 'false' in production
        //    MainPage = new NavigationPage(new StartPage());
        //}
        //else
        //{
            MainPage = GetMainPage();
        //}
    }

    public static Page GetMainPage()
    {
        return new AppFlyout();
    }

    public void ChangeFlyoutDirection()
    {
        var flyoutPage = (AppFlyout)MainPage!;
        if (AppSettings.IsRTLLanguage)
        {
            flyoutPage!.Flyout.FlowDirection = FlowDirection.RightToLeft;
            flyoutPage.FlowDirection = FlowDirection.RightToLeft;
        }
        else
        {
            flyoutPage!.Flyout.FlowDirection = FlowDirection.LeftToRight;
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

        AppTheme currentTheme = Application.Current!.RequestedTheme;

        if (AppSettings.IsUseSystemTheme)
        {
            if (currentTheme == AppTheme.Dark)
                Application.Current.Resources.ApplyDarkTheme();
            else
                Application.Current.Resources.ApplyLightTheme();
        }

        ThemeUtil.ApplyColorSet(AppSettings.SelectedPrimaryColorIndex);
    }
}