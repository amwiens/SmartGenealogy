using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.InformationalAndSocialNetworking;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class References
{
    [JsonConstructor]
    public References(
        string bioGuideId,
        string thomasId,
        string openSecretsId,
        string lisId,
        string cspanId,
        string govtrackId,
        string voteSmartId,
        string ballotpediaId,
        string washingtonPostId,
        string icpsrId,
        string wikipediaId)
    {
        BioguideId = bioGuideId;
        ThomasId = thomasId;
        OpenSecretsId = openSecretsId;
        LisId = lisId;
        CspanId = cspanId;
        GovtrackId = govtrackId;
        VotesmartId = voteSmartId;
        BallotpediaId = ballotpediaId;
        WashingtonPostId = washingtonPostId;
        IcpsrId = icpsrId;
        WikipediaId = wikipediaId;
    }

    [JsonPropertyName("bioguide_id")]
    public string BioguideId { get; set; }

    [JsonPropertyName("thomas_id")]
    public string ThomasId { get; set; }

    [JsonPropertyName("opensecrets_id")]
    public string OpenSecretsId { get; set; }

    [JsonPropertyName("lis_id")]
    public string LisId { get; set; }

    [JsonPropertyName("cspan_id")]
    public string CspanId { get; set; }

    [JsonPropertyName("govtrack_id")]
    public string GovtrackId { get; set; }

    [JsonPropertyName("votesmart_id")]
    public string VotesmartId { get; set; }

    [JsonPropertyName("ballotpedia_id")]
    public string BallotpediaId { get; set; }

    [JsonPropertyName("washington_post_id")]
    public string WashingtonPostId { get; set; }

    [JsonPropertyName("icpsr_id")]
    public string IcpsrId { get; set; }

    [JsonPropertyName("wikipedia_id")]
    public string WikipediaId { get; set; }
}