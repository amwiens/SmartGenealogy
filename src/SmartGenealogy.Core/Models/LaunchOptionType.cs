using System.Text.Json.Serialization;

namespace SmartGenealogy.Core.Models;

[JsonConverter(typeof(JsonStringEnumConverter<LaunchOptionType>))]
public enum LaunchOptionType
{
    Bool,
    String,
    Int
}