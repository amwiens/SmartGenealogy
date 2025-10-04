namespace SmartGenealogy.Data;

[JsonSerializable(typeof(FactType))]
[JsonSerializable(typeof(FactTypeJson))]
[JsonSerializable(typeof(Role))]
public partial class JsonContext : JsonSerializerContext
{
}