using System.Text.Json.Serialization;

namespace SmartGenealogy.Core.Models.Update;

[JsonSerializable(typeof(UpdateManifest))]
public record UpdateManifest
{
    public required Dictionary<UpdateChannel, UpdatePlatforms> Updates { get; init; }
}