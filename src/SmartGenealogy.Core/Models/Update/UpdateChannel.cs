using System.Text.Json.Serialization;

using SmartGenealogy.Core.Converters.Json;

namespace SmartGenealogy.Core.Models.Update;

[JsonConverter(typeof(DefaultUnknownEnumConverter<UpdateChannel>))]
public enum UpdateChannel
{
    Unknown,
    Stable,
    Preview,
    Development
}