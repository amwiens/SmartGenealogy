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
            .UseOcr()
            .ConfigureRepositories()
            .ConfigureServices()
            .ConfigureViewModels()
            .ConfigureViews()
            .ConfigurePopups()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

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

        mauiAppBuilder.Services.AddSingleton(OcrPlugin.Default);

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
        mauiAppBuilder.Services.AddSingleton<MultimediaPage>();
        mauiAppBuilder.Services.AddTransient<PlacesPage>();
        mauiAppBuilder.Services.AddTransient<SettingsPage>();
        mauiAppBuilder.Services.AddTransient<ToolsPage>();
        mauiAppBuilder.Services.AddTransientWithShellRoute<FactTypesPage, FactTypesPageViewModel>("factTypes");
        mauiAppBuilder.Services.AddTransientWithShellRoute<FactTypePage, FactTypePageViewModel>("factTypeDetails");
        mauiAppBuilder.Services.AddTransientWithShellRoute<MultimediaDetailsPage, MultimediaDetailsPageViewModel>("multimediaDetails");
        mauiAppBuilder.Services.AddTransientWithShellRoute<PlaceDetailsPage, PlaceDetailsPageViewModel>("placeDetails");
        mauiAppBuilder.Services.AddTransientWithShellRoute<PlacePage, PlacePageViewModel>("place");

        return mauiAppBuilder;
    }

    /// <summary>
    /// Configure popups for dependency injection.
    /// </summary>
    private static MauiAppBuilder ConfigurePopups(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransientPopup<NewDatabasePopup, NewDatabasePopupViewModel>();
        mauiAppBuilder.Services.AddTransientPopup<AddEditFactTypePopup, AddEditFactTypePopupViewModel>();
        mauiAppBuilder.Services.AddTransientPopup<AddEditMultimediaPopup, AddEditMultimediaPopupViewModel>();
        mauiAppBuilder.Services.AddTransientPopup<AddEditPlacePopup, AddEditPlacePopupViewModel>();
        mauiAppBuilder.Services.AddTransientPopup<AddEditRolePopup, AddEditRolePopupViewModel>();

        return mauiAppBuilder;
    }
}