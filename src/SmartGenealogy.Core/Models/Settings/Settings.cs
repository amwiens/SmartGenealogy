using System.Globalization;
using System.Text.Json.Serialization;

namespace SmartGenealogy.Core.Models.Settings;

public class Settings
{
    public int? Version { get; set; } = 1;
    public bool FirstLaunchSetupComplete { get; set; }
    public string? Theme { get; set; } = "Dark";
    public string? Language { get; set; } = GetDefaultCulture().Name;

    public NumberFormatMode NumberFormatMode { get; set; } = NumberFormatMode.CurrentCulture;


    public WindowSettings? WindowSettings { get; set; }



    public Dictionary<NotificationKey, NotificationOption> NotificationOptions { get; set; } = new();



    /// <summary>
    /// Return either the system default culture, if supported, or en-US
    /// </summary>
    /// <returns></returns>
    public static CultureInfo GetDefaultCulture()
    {
        var supportedCultures = new[] { "en-US", "ja-JP", "zh-Hans", "zh-Hant" };

        var systemCulture = CultureInfo.InstalledUICulture;

        return supportedCultures.Contains(systemCulture.Name) ? systemCulture : new CultureInfo("en-US");
    }
}

[JsonSourceGenerationOptions(
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(Settings))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(string))]
internal partial class SettingsSerializerContext : JsonSerializerContext;