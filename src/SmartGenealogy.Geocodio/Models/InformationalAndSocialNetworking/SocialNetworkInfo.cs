using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.InformationalAndSocialNetworking;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class SocialNetworkInfo
{
    [JsonConstructor]
    public SocialNetworkInfo(
        string rssURL,
        string twitter,
        string facebook,
        string youtube,
        string youtubeId)
    {
        RssURL = rssURL;
        Twitter = twitter;
        Facebook = facebook;
        YouTube = youtube;
        YouTubeId = youtubeId;
    }

    [JsonPropertyName("rss_url")]
    public string RssURL { get; set; }
    public string Twitter { get; set; }
    public string Facebook { get; set; }
    public string YouTube { get; set; }
    [JsonPropertyName("youtube_id")]
    public string YouTubeId { get; set; }
}