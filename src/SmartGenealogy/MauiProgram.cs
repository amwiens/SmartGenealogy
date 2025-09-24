using Microsoft.Maui.LifecycleEvents;

using SkiaSharp.Views.Maui.Controls.Hosting;

using PanCardView;

using FFImageLoading.Maui;

using RGPopup.Maui.Extensions;

using SmartGenealogy.Handlers;
using SmartGenealogy.Views.Media;
using Microsoft.Maui.Platform;




#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;

using Windows.Graphics;

using WinRT.Interop;

using Windows.UI.Core;
#endif

namespace SmartGenealogy;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        SmartGenealogySettings.LoadSettings();

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .RegisterAppServices()
            .RegisterRepositories()
            .RegisterViewModels()
            .RegisterViews()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .UseSkiaSharp()
            .UseCardsView()
            .UseFFImageLoading()
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
            })
                        .ConfigureMauiHandlers(handlers =>
                        {
                            handlers.AddHandler(typeof(ProgressBar), typeof(ProgressBarHandler));
                        })
            .ConfigureLifecycleEvents(events =>
            {
#if ANDROID
                events.AddAndroid(android => android.OnCreate((activity, bundle) => MakeStatusBarTranslucent(activity)));

                static void MakeStatusBarTranslucent(Android.App.Activity activity)
                {
                    //activity.Window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);

                    activity.Window!.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);

                    activity.Window.SetStatusBarColor(Android.Graphics.Color.White);

                    activity.Window.SetNavigationBarColor(Android.Graphics.Color.Transparent);
                }
#endif
            });

#if DEBUG
        builder.Logging.AddDebug();
        builder.Services.AddLogging(configure => configure.AddDebug());
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

                    //https://github.com/dotnet/maui/issues/7751
                    window.ExtendsContentIntoTitleBar = false; // must be false or else you see some of the buttons
                    winuiAppWindow.SetPresenter(AppWindowPresenterKind.Default);

                    //https://github.com/microsoft/microsoft-ui-xaml/issues/8746
                    ///
                    /// System back button for backwards navigation is no longer recommend
                    /// Instead, you should provide your own in-app back button
                    //  https://learn.microsoft.com/en-us/windows/apps/design/basics/navigation-history-and-backwards-navigation?source=recommendations#system-back-behavior-for-backward-compatibility
                    var titleBar = winuiAppWindow.TitleBar;
                    titleBar.ExtendsContentIntoTitleBar = true;
                    titleBar.BackgroundColor = Windows.UI.Color.FromArgb(1, 186, 213, 248); //Hex: #BAD5F8
                    titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(1, 186, 213, 248); //Hex: #BAD5F8
                    titleBar.ButtonInactiveBackgroundColor = Microsoft.UI.Colors.Black;
                    titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(1, 186, 213, 248); //Hex: #BAD5F8
                    titleBar.ForegroundColor = Microsoft.UI.Colors.White;
                    titleBar.InactiveBackgroundColor = Microsoft.UI.Colors.Black;

                    //https://github.com/dotnet/maui/issues/6976
                    var displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(win32WindowsId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);

                    int width = displayArea.WorkArea.Width * 3 / 4;
                    int height = displayArea.WorkArea.Height - 50;
                    var windowState = SmartGenealogySettings.WindowState;

                    if (windowState is not null)
                        winuiAppWindow.MoveAndResize(new RectInt32((int)windowState.X, (int)windowState.Y, (int)windowState.Width, (int)windowState.Height));
                    else
                        winuiAppWindow.MoveAndResize(new RectInt32(25, 50, width, height));
                });
            });
            events.AddWindows(windows => windows
                .OnClosed((window, args) =>
                {
                    IWindow appWindow = window.GetWindow()!;
                    SmartGenealogySettings.WindowState!.X = appWindow.X;
                    SmartGenealogySettings.WindowState!.Y = appWindow.Y;
                    SmartGenealogySettings.WindowState!.Width = appWindow.Width;
                    SmartGenealogySettings.WindowState!.Height = appWindow.Height;
                    SmartGenealogySettings.SaveSettings();
                }));
#endif
        });
        builder.Services.AddLocalization();

        var app = builder.Build();

        return app;
    }

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterRepositories(this MauiAppBuilder mauiAppBuilder)
    {
        // Register repositories
        mauiAppBuilder.Services.AddSingleton<FactTypeRepository>();

        mauiAppBuilder.Services.AddSingleton<DatabaseSettings>();
        mauiAppBuilder.Services.AddSingleton<DatabaseInitializer>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        // Register view models
        mauiAppBuilder.Services.AddTransient<AppShellViewModel>();
        mauiAppBuilder.Services.AddSingleton<MainPageViewModel>();
        mauiAppBuilder.Services.AddTransient<MediaPageViewModel>();
        mauiAppBuilder.Services.AddTransient<SettingsPageViewModel>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        // Register views
        mauiAppBuilder.Services.AddTransient<AppShell>();
        mauiAppBuilder.Services.AddTransient<MainPage>();
        mauiAppBuilder.Services.AddTransientWithShellRoute<MediaDetailsPage, MediaDetailsPageViewModel>("mediaDetails");

        return mauiAppBuilder;
    }
}