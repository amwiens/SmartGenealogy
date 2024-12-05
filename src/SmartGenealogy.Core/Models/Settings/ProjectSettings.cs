using System.Text.Json.Serialization;

namespace SmartGenealogy.Core.Models.Settings;

public class ProjectSettings
{
    public string? Name { get; set; }

    public string? DatabasePath { get; set; }
}

[JsonSourceGenerationOptions(
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(ProjectSettings))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(string))]
internal partial class ProjectSettingsSerializerContext : JsonSerializerContext;