using CommunityToolkit.Maui;

using Microsoft.Extensions.Logging;

using SmartGenealogy.Images.Services;
using SmartGenealogy.Services;
using SmartGenealogy.ViewModels;
using SmartGenealogy.ViewModels.Media;
using SmartGenealogy.ViewModels.Places;
using SmartGenealogy.ViewModels.Settings;
using SmartGenealogy.Views.Media;
using SmartGenealogy.Views.Places;
using SmartGenealogy.Views.Settings;

namespace SmartGenealogy;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Services
		builder.Services.AddSingleton<GeocodeService>();
		builder.Services.AddSingleton<ImageService>();
		builder.Services.AddSingleton<MultimediaService>();
		builder.Services.AddSingleton<MediaLinkService>();
		builder.Services.AddSingleton<PlaceService>();
        builder.Services.AddSingleton<PlaceDetailService>();

        // ViewModels
        builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<PlacesViewModel>();
		builder.Services.AddTransient<PlaceViewModel>();
		builder.Services.AddTransient<PlaceDetailViewModel>();
		builder.Services.AddTransient<AddPlaceViewModel>();
        builder.Services.AddTransient<AddPlaceDetailViewModel>();
        builder.Services.AddTransient<EditPlaceViewModel>();
		builder.Services.AddTransient<EditPlaceDetailViewModel>();
		builder.Services.AddSingleton<MediaViewModel>();
		builder.Services.AddTransient<AddMediaViewModel>();
		builder.Services.AddTransient<SettingsViewModel>();
		builder.Services.AddTransient<AISettingsViewModel>();
		builder.Services.AddTransient<ImageSettingsViewModel>();
		builder.Services.AddTransient<PlaceSettingsViewModel>();

        // Pages
        builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<PlacesPage>();
		builder.Services.AddTransient<PlacePage>();
		builder.Services.AddTransient<AddPlacePage>();
        builder.Services.AddTransient<AddPlaceDetailPage>();
        builder.Services.AddTransient<EditPlacePage>();
		builder.Services.AddTransient<EditPlaceDetailPage>();
		builder.Services.AddSingleton<MediaPage>();
		builder.Services.AddTransient<AddMediaPage>();
		builder.Services.AddTransient<SettingsPage>();
		builder.Services.AddTransient<AISettingsPage>();
		builder.Services.AddTransient<ImageSettingsPage>();
		builder.Services.AddTransient<PlaceSettingsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}