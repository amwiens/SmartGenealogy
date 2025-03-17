using System.Text.Json.Serialization;

namespace SmartGenealogy.Models;

public class DownloadResponse
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    [JsonPropertyName("digest")]
    public string? Digest { get; set; }
    [JsonPropertyName("total")]
    public long? Total { get; set; }
    [JsonPropertyName("completed")]
    public long? Completed { get; set; }
}