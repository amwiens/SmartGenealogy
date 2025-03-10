﻿using System.Globalization;
using System.Text.Json.Serialization;

namespace SmartGenealogy.Core.Models.Settings;

public class Settings
{
    public int? Version { get; set; } = 1;

    public string? Theme { get; set; } = "Dark";
    public string? Language { get; set; } = GetDefaultCulture().Name;

    public string? OllamaUrl { get; set; } = "http://localhost:11434";

    public NumberFormatMode NumberFormatMode { get; set; } = NumberFormatMode.CurrentCulture;

    public WindowSettings? WindowSettings { get; set; }

    public float AnimationScale { get; set; } = 1.0f;

    public Dictionary<NotificationKey, NotificationOption> NotificationOptions { get; set; } = new();



    /// <summary>
    /// Return either the system default culture, if supported, or en-US
    /// </summary>
    /// <returns></returns>
    public static CultureInfo GetDefaultCulture()
    {
        var supportedCultures = new[] { "en-US" };

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
internal partial class SettingsSerialzerContext : JsonSerializerContext;