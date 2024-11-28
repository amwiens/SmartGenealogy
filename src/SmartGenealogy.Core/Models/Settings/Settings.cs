using System.Globalization;
using System.Text.Json.Serialization;

using Semver;

using SmartGenealogy.Core.Converters.Json;
using SmartGenealogy.Core.Models.Update;

namespace SmartGenealogy.Core.Models.Settings;

public class Settings
{
    public int? Version { get; set; } = 1;
    public bool FirstLaunchSetupComplete { get; set; }
    public string? Theme { get; set; } = "Dark";
    public string? Language { get; set; } = GetDefaultCulture().Name;

    public NumberFormatMode NumberFormatMode { get; set; } = NumberFormatMode.CurrentCulture;



    /// <summary>
    /// Preferred update channel
    /// </summary>
    public UpdateChannel PreferredUpdateChannel { get; set; } = UpdateChannel.Stable;

    /// <summary>
    /// Whether to check for updates
    /// </summary>
    public bool CheckForUpdates { get; set; } = true;

    /// <summary>
    /// The last auto-update version that had a notification dismissed by the user
    /// </summary>
    [JsonConverter(typeof(SemVersionJsonConverter))]
    public SemVersion? LastSeenUpdateVersion { get; set; }

    /// <summary>
    /// Set to the version the user is updating from when updating
    /// </summary>
    [JsonConverter(typeof(SemVersionJsonConverter))]
    public SemVersion? UpdatingFromVersion { get; set; }



    public WindowSettings? WindowSettings { get; set; }



    public bool RemoveFolderLinksOnShutdown { get; set; }



    public float AnimationScale { get; set; } = 1.0f;



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