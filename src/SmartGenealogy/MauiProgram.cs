namespace SmartGenealogy;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        SmartGenealogySettings.LoadSettings();

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Register repositories


        builder.Services.AddSingleton<DatabaseSettings>();

        // Register view models
        builder.Services.AddTransient<AppShellViewModel>();
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<MediaPageViewModel>();
        builder.Services.AddTransient<SettingsPageViewModel>();

        // Register views
        builder.Services.AddTransient<AppShell>();
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
    }
}