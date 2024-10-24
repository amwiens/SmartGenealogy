using System;

using Avalonia;

using Projektanker.Icons.Avalonia.FontAwesome;
using Projektanker.Icons.Avalonia;

namespace SmartGenealogy;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }



    internal static void SetupAvaloniaApp()
    {
        IconProvider.Current.Register<FontAwesomeIconProvider>();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        SetupAvaloniaApp();
        var app = AppBuilder.Configure<App>().UsePlatformDetect().WithInterFont().LogToTrace();



        return app;
    }
}
