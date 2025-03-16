using System.Text.Json;

using SmartGenealogy.Models;

namespace SmartGenealogy.Services;

public static class SettingsManager
{
    private static AppSettings? _settings;

    public static AppSettings LoadSettings()
    {
        if (_settings == null)
        {
            var json = File.ReadAllText(Path.Combine(FileSystem.AppDataDirectory, "AppSettings.json"));
            _settings = JsonSerializer.Deserialize<AppSettings>(json)!;
        }
        return _settings;
    }

    public static void SaveSettings(AppSettings settings)
    {
        var json = JsonSerializer.Serialize(settings);
        File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, "AppSettings.json"), json);
    }
}