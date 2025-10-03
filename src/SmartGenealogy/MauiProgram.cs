namespace SmartGenealogy;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureRepositories()
            .ConfigureServices()
            .ConfigureViewModels()
            .ConfigureViews()
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

    private static MauiAppBuilder ConfigureRepositories(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<FactTypeRepository>();
        mauiAppBuilder.Services.AddSingleton<DatabaseSettings>();

        return mauiAppBuilder;
    }

    private static MauiAppBuilder ConfigureServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<ModalErrorHandler>();

        return mauiAppBuilder;
    }

    private static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<MainPageViewModel>();

        return mauiAppBuilder;
    }

    private static MauiAppBuilder ConfigureViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<MainPage>();

        return mauiAppBuilder;
    }
}