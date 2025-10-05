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
            .UseMauiCommunityToolkit()
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
        mauiAppBuilder.Services.AddSingleton<FactTypeRepository>();
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

        return mauiAppBuilder;
    }

    /// <summary>
    /// Configure view models for dependency injection.
    /// </summary>
    private static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<AppShellViewModel>();
        mauiAppBuilder.Services.AddTransient<MainPageViewModel>();
        mauiAppBuilder.Services.AddTransient<SettingsPageViewModel>();

        return mauiAppBuilder;
    }

    /// <summary>
    /// Configure views for dependency injection.
    /// </summary>
    private static MauiAppBuilder ConfigureViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<AppShell>();
        mauiAppBuilder.Services.AddTransient<MainPage>();
        mauiAppBuilder.Services.AddTransient<SettingsPage>();

        return mauiAppBuilder;
    }

    /// <summary>
    /// Configure popups for dependency injection.
    /// </summary>
    private static MauiAppBuilder ConfigurePopups(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransientPopup<NewDatabasePopup, NewDatabasePopupViewModel>();

        return mauiAppBuilder;
    }
}