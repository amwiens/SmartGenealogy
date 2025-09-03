using System.Text.Json.Serialization;

namespace SmartGenealogy.Data.Data;

[JsonSerializable(typeof(FactType))]
[JsonSerializable(typeof(FactTypesJson))]
public partial class JsonContext : JsonSerializerContext
{
}