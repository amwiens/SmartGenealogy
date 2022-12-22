namespace SmartGenealogy.Mobile.ViewModels;

public static class ViewModelExtensions
{
    public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddSingleton<ShellViewModel>();

        return builder;
    }
}