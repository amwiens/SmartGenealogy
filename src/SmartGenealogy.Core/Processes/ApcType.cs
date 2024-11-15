using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SmartGenealogy.Core.Processes;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApcType
{
    [EnumMember(Value = "input")]
    Input = 1,
}