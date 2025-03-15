using System.Text.Json.Serialization;

using SmartGenealogy.Geocodio.Models.InformationalAndSocialNetworking;

namespace SmartGenealogy.Geocodio.Models.FederalLegislature;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class FederalLegislator
{
    [JsonConstructor]
    public FederalLegislator(
        string type,
        Bio bio,
        ContactInfo contactInfo,
        SocialNetworkInfo socialNetworkInfo,
        References references,
        string source)
    {
        Type = type;
        Bio = bio;
        ContactInfo = contactInfo;
        SocialNetworkInfo = socialNetworkInfo;
        References = references;
        Source = source;
    }

    public string Type { get; set; }
    public Bio Bio { get; set; }

    [JsonPropertyName("contact")]
    public ContactInfo ContactInfo { get; set; }

    [JsonPropertyName("social")]
    public SocialNetworkInfo SocialNetworkInfo { get; set; }

    public References References { get; set; }
    public string Source { get; set; }
}