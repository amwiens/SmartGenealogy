using Microsoft.Maui.LifecycleEvents;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

namespace SmartGenealogy;

/// <summary>
/// Main Maui program class.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Creates the Maui app.
    /// </summary>
    public static MauiApp CreateMauiApp()
    {
        SmartGenealogySettings.LoadSettings();

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit(options =>
            {
                options.SetShouldEnableSnackbarOnWindows(true);
            })
            .UseMauiCommunityToolkitMediaElement()
            .UseOcr()
            .ConfigureRepositories()
            .ConfigureServices()
            .ConfigureViewModels()
            .ConfigureViews()
            .ConfigurePopups()
            .UseSkiaSharp()
            .UseCardsView()
            .UseFFImageLoading()
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
                handlers.AddHandler(typeof(ProgressBar), typeof(Microsoft.Maui.Handlers.ProgressBarHandler));
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

                    // https://github.com/dotnet/maui/issues/7751
                    window.ExtendsContentIntoTitleBar = false; // must be false or else you see some of the buttons
                    winuiAppWindow.SetPresenter(AppWindowPresenterKind.Default);

                    // https://github.com/microsoft/microsoft-ui-xaml/issues/8746
                    ///
                    /// System back button for backwards navigation is no longer recommended.
                    /// Instead you should provide your own in-app back button
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

                    winuiAppWindow.MoveAndResize(new RectInt32(25, 50, width, height));
                });
            });
#endif
        });

        builder.Services.AddLocalization();

        return builder.Build();
    }

    /// <summary>
    /// Configure repositories for dependency injection.
    /// </summary>
    private static MauiAppBuilder ConfigureRepositories(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<DatabaseSettings>();
        mauiAppBuilder.Services.AddSingleton<DatabaseTools>();

        mauiAppBuilder.Services.AddSingleton<FactTypeRepository>();
        mauiAppBuilder.Services.AddSingleton<MediaLinkRepository>();
        mauiAppBuilder.Services.AddSingleton<MultimediaLineRepository>();
        mauiAppBuilder.Services.AddSingleton<MultimediaRepository>();
        mauiAppBuilder.Services.AddSingleton<MultimediaWordRepository>();
        mauiAppBuilder.Services.AddSingleton<PlaceRepository>();
        mauiAppBuilder.Services.AddSingleton<RoleRepository>();
        mauiAppBuilder.Services.AddSingleton<WebLinkRepository>();

        return mauiAppBuilder;
    }

    /// <summary>
    /// Configure services for dependency injection.
    /// </summary>
    private static MauiAppBuilder ConfigureServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<SeedDataService>();
        mauiAppBuilder.Services.AddSingleton<ModalErrorHandler>();
        mauiAppBuilder.Services.AddSingleton<OCRService>();
        mauiAppBuilder.Services.AddSingleton<IAlertService, AlertService>();
        mauiAppBuilder.Services.AddSingleton<IFactTypeService, FactTypeService>();
        mauiAppBuilder.Services.AddSingleton<IMultimediaService, MultimediaService>();
        mauiAppBuilder.Services.AddSingleton<IPlaceService, PlaceService>();

        mauiAppBuilder.Services.AddSingleton(OcrPlugin.Default);
        mauiAppBuilder.Services.AddSingleton<LocationIQService>();

        return mauiAppBuilder;
    }

    /// <summary>
    /// Configure view models for dependency injection.
    /// </summary>
    private static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<AppShellViewModel>();
        mauiAppBuilder.Services.AddSingleton<MainPageViewModel>();
        mauiAppBuilder.Services.AddSingleton<MultimediaPageViewModel>();
        mauiAppBuilder.Services.AddTransient<PlacesPageViewModel>();
        mauiAppBuilder.Services.AddTransient<SettingsPageViewModel>();
        mauiAppBuilder.Services.AddTransient<ToolsPageViewModel>();

        return mauiAppBuilder;
    }

    /// <summary>
    /// Configure views for dependency injection.
    /// </summary>
    private static MauiAppBuilder ConfigureViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<AppShell>();
        mauiAppBuilder.Services.AddSingleton<MainPage>();
        mauiAppBuilder.Services.AddTransient<PlacesPage>();
        mauiAppBuilder.Services.AddTransientWithShellRoute<PlaceDetailsPage, PlaceDetailsPageViewModel>("placeDetails");
        mauiAppBuilder.Services.AddTransientWithShellRoute<PlacePage, PlacePageViewModel>("place");
        mauiAppBuilder.Services.AddSingleton<MultimediaPage>();
        mauiAppBuilder.Services.AddTransientWithShellRoute<MultimediaDetailsPage, MultimediaDetailsPageViewModel>("multimediaDetails");
        mauiAppBuilder.Services.AddTransient<ToolsPage>();
        mauiAppBuilder.Services.AddTransientWithShellRoute<FactTypesPage, FactTypesPageViewModel>("factTypes");
        mauiAppBuilder.Services.AddTransientWithShellRoute<FactTypePage, FactTypePageViewModel>("factTypeDetails");
        mauiAppBuilder.Services.AddTransientWithShellRoute<WebLinksPage, WebLinksPageViewModel>("webLinks");
        mauiAppBuilder.Services.AddTransient<SettingsPage>();
        mauiAppBuilder.Services.AddTransientWithShellRoute<GeocodeSettingsPage, GeocodeSettingsPageViewModel>("geocodeSettings");

        return mauiAppBuilder;
    }

    /// <summary>
    /// Configure popups for dependency injection.
    /// </summary>
    private static MauiAppBuilder ConfigurePopups(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransientPopup<AddEditFactTypePopup, AddEditFactTypePopupViewModel>();
        mauiAppBuilder.Services.AddTransientPopup<AddEditMultimediaPopup, AddEditMultimediaPopupViewModel>();
        mauiAppBuilder.Services.AddTransientPopup<AddEditPlacePopup, AddEditPlacePopupViewModel>();
        mauiAppBuilder.Services.AddTransientPopup<AddEditRolePopup, AddEditRolePopupViewModel>();
        mauiAppBuilder.Services.AddTransientPopup<AddEditWebLinkPopup, AddEditWebLinkPopupViewModel>();
        mauiAppBuilder.Services.AddTransientPopup<NewDatabasePopup, NewDatabasePopupViewModel>();
        mauiAppBuilder.Services.AddTransientPopup<SelectMultimediaPopup, SelectMultimediaPopupViewModel>();

        return mauiAppBuilder;
    }
}