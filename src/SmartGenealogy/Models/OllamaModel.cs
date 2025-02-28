using System.Text.Json.Serialization;

namespace SmartGenealogy.Models;

public class OllamaModel
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("tags")]
    public string[]? Tags { get; set; }
}