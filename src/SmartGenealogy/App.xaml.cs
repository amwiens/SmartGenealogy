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

namespace SmartGenealogy;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AppShell _appShell;

    public App(IServiceProvider serviceProvider)
    {
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

        _serviceProvider = serviceProvider;
    }

    public Page GetMainPage()
    {
        var appShell = _serviceProvider.GetRequiredService<AppShell>();
        return appShell;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(GetMainPage());
        var windowState = SmartGenealogySettings.WindowState;
        if (windowState is not null)
        {
            window.X = windowState.X;
            window.Y = windowState.Y;
            window.Width = windowState.Width;
            window.Height = windowState.Height;
        }
        return window;
    }

    protected override void OnStart()
    {
        base.OnStart();

        var appLanguageCode = string.Empty;

        var systemLanguageCode = CultureInfo.CurrentUICulture.Name;

        LocalizationResourceManager.Instance.SetCulture(new CultureInfo(appLanguageCode));

        if (SmartGenealogySettings.UseDarkMode)
            Application.Current!.Resources.ApplyDarkTheme();
        else
            Application.Current!.Resources.ApplyLightTheme();
    }
}