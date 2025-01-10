using CommunityToolkit.Maui;

using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

using PanCardView;

using RGPopup.Maui.Extensions;

using SkiaSharp.Views.Maui.Controls.Hosting;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;

using Windows.Graphics;
#endif

namespace SmartGenealogy;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .RegisterAppServices()
            .RegisterViewModels()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .UseSkiaSharp()
            .UseCardsView()
            .UseMauiRGPopup(config =>
            {
                config.BackPressHandler = null;
                config.FixKeyboardOverlap = true;
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Poppins-Regular.otf", "RegularFont");
                fonts.AddFont("Poppins-Medium.otf", "MediumFont");
                fonts.AddFont("Poppins-SemiBold.otf", "SemiBoldFont");
                fonts.AddFont("Poppins-Bold.otf", "BoldFont");
                fonts.AddFont("Caveat-Bold.ttf", "HandwritingBoldFont");
                fonts.AddFont("Caveat-Regular.ttf", "HandwritingFont");

                fonts.AddFont("fa-solid-900.ttf", "FaPro");
                fonts.AddFont("fa-brands-400.ttf", "FaBrands");
                fonts.AddFont("fa-regular-400.ttf", "FaRegular");
                fonts.AddFont("line-awesome.ttf", "LineAwesome");
                fonts.AddFont("material-icons-outlined-regular.otf", "MaterialDesign");
                fonts.AddFont("ionicons.ttf", "IonIcons");
                fonts.AddFont("icon.ttf", "SmartGenealogyIcons");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.ConfigureLifecycleEvents(events =>
        {
#if WINDOWS
            events.AddWindows(wndLifeCycleBuilder =>
            {
                wndLifeCycleBuilder.OnWindowCreated(window =>
                {
                    IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                    AppWindow winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);

                    // https://github.com/dotnet/maui/issues/7751
                    window.ExtendsContentIntoTitleBar = false; // must be false or else you see some of the buttons
                    winuiAppWindow.SetPresenter(AppWindowPresenterKind.Default);

                    // https://github.com/microsoft/microsoft-ui-xaml/issues/8746
                    /// System back button for backwards navigation is no longer recommended
                    /// Instead, you should provide your own in-app back button
                    // https://learn.microsoft.com/en-us/windows/apps/design/basics/navigation-history-and-backwards-navigation?source=recommendations#system-back-behavior-for-backward-compatibility
                    var titleBar = winuiAppWindow.TitleBar;
                    titleBar.ExtendsContentIntoTitleBar = true;
                    titleBar.BackgroundColor = Windows.UI.Color.FromArgb(1, 186, 213, 248); // Hex: #BAD5F8
                    titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(1, 186, 213, 248); // Hex: #BAD5F8
                    titleBar.ButtonInactiveBackgroundColor = Microsoft.UI.Colors.Black;
                    titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(1, 186, 213, 248); // Hex: #BAD5F8
                    titleBar.ForegroundColor = Microsoft.UI.Colors.White;
                    titleBar.InactiveBackgroundColor = Microsoft.UI.Colors.Black;

                    // https://github.com/dotnet/maui/issues/6976
                    var displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(win32WindowsId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);

                    int width = displayArea.WorkArea.Width * 3 / 4;
                    int height = displayArea.WorkArea.Height - 50;

                    winuiAppWindow.MoveAndResize(new RectInt32(25, 50, width, height));
                });
            });
#endif
        });

        builder.Services.AddLocalization();

        var app = builder.Build();

        return app;
    }

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<AppFlyout>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<MainMenuPage>();
        mauiAppBuilder.Services.AddTransient<MainPage>();

        mauiAppBuilder.Services.AddTransient<MainMenuViewModel>();
        mauiAppBuilder.Services.AddTransient<MainViewModel>();

        mauiAppBuilder.Services.AddTransient<OllamaSettingsViewModel>();

        return mauiAppBuilder;
    }
}