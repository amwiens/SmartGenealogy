namespace SmartGenealogy.Core.Settings;

/// <summary>
/// Used to (de)serialize the settings.json file.
/// </summary>
public record SettingsRecord
{
    public bool OpenLastDatabaseOnStartup { get; init; } = false;
    public string? LastOpenDatabase { get; init; } = string.Empty;
}

/// <summary>
/// Provides application-wide settings management for the Smart Genealogy application.
/// 
/// Handles the loading, saving, and updating of user preferences and settings.
/// </summary>
public static class SmartGenealogySettings
{
    private static readonly JsonSerializerOptions SerializerOptions =
        new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
            AllowTrailingCommas = true,
        };

    private static readonly string SettingsFileDir =
        Path.Combine("c:\\Code\\Mine\\", "Settings");

    /// <summary>
    /// Returns the default set of settings for the <see cref="SmartGenealogy"/> app.
    /// </summary>
    public static SettingsRecord DefaultSettings => new();


    public static SettingsRecord CurrentSettings =>
        new()
        {
            OpenLastDatabaseOnStartup = OpenLastDatabaseOnStartup,
            LastOpenDatabase = LastOpenDatabase,
        };

    /// <summary>
    /// The full path to settings.json.
    /// </summary>
    public static readonly string SettingsFilePath = Path.Combine(SettingsFileDir, "settings.json");
    private static string _previousSettingsJson = string.Empty;

    /// <summary>
    /// Indicates whether the application opens the last database on startup. Defaults to <see langword="false"/>.
    /// </summary>
    public static bool OpenLastDatabaseOnStartup { get; set; }

    /// <summary>
    /// Holds the value of the last oepn database.
    /// </summary>
    public static string? LastOpenDatabase { get; set; }

    /// <summary>
    /// Loads settings from the settings file and applies them to the current session.
    /// 
    /// If the settings file does not exist or is invalid, default settings are used and settings.json is rewritten.
    /// </summary>
    public static void LoadSettings()
    {
        if (!File.Exists(SettingsFilePath))
        {
            ImportSettings(DefaultSettings);
            SaveSettings();
            return;
        }

        _previousSettingsJson = File.ReadAllText(SettingsFilePath, Encoding.UTF8);
        try
        {
            SettingsRecord settings =
                JsonSerializer.Deserialize<SettingsRecord>(_previousSettingsJson, SerializerOptions)
                ?? throw new InvalidDataException();

            ImportSettings(settings);
        }
        catch
        {
            ImportSettings(DefaultSettings);
            SaveSettings();
        }
    }

    /// <summary>
    /// Loads settings from the <see cref="SettingsRecord"/> object and applies them to the current session.
    /// </summary>
    /// <param name="settings">Settings</param>
    public static void ImportSettings(SettingsRecord settings)
    {
        OpenLastDatabaseOnStartup = settings.OpenLastDatabaseOnStartup;
        LastOpenDatabase = settings.LastOpenDatabase;
    }

    /// <summary>
    /// Saves the current settings to the settings file if any changes have been made.
    /// </summary>
    public static void SaveSettings()
    {
        SettingsRecord settings = CurrentSettings;
        string settingsJson = JsonSerializer.Serialize(settings, SerializerOptions);

        if (!Directory.Exists(SettingsFileDir))
            Directory.CreateDirectory(SettingsFileDir);

        if (_previousSettingsJson != settingsJson)
            File.WriteAllText(SettingsFilePath, settingsJson, Encoding.UTF8);
    }
}