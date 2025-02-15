using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

using SmartGenealogy.Core.Converters.Json;

namespace SmartGenealogy.Core.Models.Settings;

/// <summary>
/// Notification Names
/// </summary>
[JsonConverter(typeof(ParsableStringValueJsonConverter<NotificationKey>))]
public record NotificationKey(string Value) : StringValue(Value), IParsable<NotificationKey>
{
    public NotificationOption DefaultOption { get; init; }

    public NotificationLevel Level { get; init; }

    public string? DisplayName { get; init; }



    public static Dictionary<string, NotificationKey> All { get; } = GetValues<NotificationKey>();

    /// <inheritdoc />
    public override string ToString() => base.ToString();

    /// <inheritdoc />
    public static NotificationKey Parse(string s, IFormatProvider? provider)
    {
        return All[s];
    }

    /// <inheritdoc />
    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out NotificationKey result)
    {
        return All.TryGetValue(s ?? "", out result);
    }
}