using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SmartGenealogy.DesignData;

[Localizable(false)]
public static class DesignData
{
    [NotNull]
    public static IServiceProvider? Services { get; set; }

    private static bool isInitialized;

    // This needs to be static method instead of static constructor
    // or else Avalonia analyzers won't work.
    public static void Initialize()
    {
        if (isInitialized)
            throw new InvalidOperationException("DesignData is already initialized.");

        var services = App.ConfigureServices();
    }
}