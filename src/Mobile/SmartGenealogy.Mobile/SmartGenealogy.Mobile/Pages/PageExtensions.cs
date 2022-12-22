namespace SmartGenealogy.Mobile.Pages;

public static class PageExtensions
{
    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        // main tabs of the app
        builder.Services.AddSingleton<HomePage>();

        return builder;
    }
}