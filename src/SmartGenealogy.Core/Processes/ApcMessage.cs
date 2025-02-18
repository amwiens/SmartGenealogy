using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

using NLog;

namespace SmartGenealogy.Core.Processes;

/// <summary>
/// Defines a custom APC message, embeddable in a subprocess output stream.
/// Format is as such:
/// <code>"{APC}{CustomPrefix}(JsonSerialized ApcMessage){StChar}"</code>
/// <example>"\u009f[SG;{"type":"input","data":"hello"}\u009c"</example>
/// </summary>
public readonly record struct ApcMessage
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public const char ApcChar = (char)0x9F;
    public const char StChar = (char)0x9C;
    public const string CustomPrefix = "[SG;";

    [JsonPropertyName("type")]
    public required ApcType Type { get; init; }

    [JsonPropertyName("data")]
    public required string Data { get; init; }


    public static bool TryParse(string value, [NotNullWhen(true)] out ApcMessage? message)
    {
        message = null;
        var startIndex = value.IndexOf(ApcChar);
        if (startIndex == -1) return false;

        // Check the IdPrefix follows the ApcEscape
        var idIndex = value.IndexOf(CustomPrefix, startIndex + 1, StringComparison.Ordinal);
        if (idIndex == -1) return false;

        // Get the end index (ST escape)
        var stIndex = value.IndexOf(StChar, idIndex + CustomPrefix.Length);
        if (stIndex == -1) return false;

        // Extract the json string (between idIndex and stIndex
        var json = value.Substring(idIndex + CustomPrefix.Length, stIndex - idIndex - CustomPrefix.Length);

        try
        {
            message = JsonSerializer.Deserialize<ApcMessage>(json);
            return true;
        }
        catch (Exception ex)
        {
            Logger.Warn($"Failed to deserialize APC message: {ex.Message}");
            return false;
        }
    }
}