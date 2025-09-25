namespace SmartGenealogy.Data;

[JsonSerializable(typeof(FactType))]
[JsonSerializable(typeof(FactTypesJson))]
public partial class JsonContext : JsonSerializerContext
{
}