﻿using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.Models.InformationalAndSocialNetworking;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class Bio
{
    [JsonConstructor]
    public Bio(
        string lastname,
        string firstname,
        string birthday,
        string gender,
        string party)
    {
        LastName = lastname;
        FirstName = firstname;
        Birthday = birthday;
        Gender = gender;
        Party = party;
    }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    public string Birthday { get; set; }
    public string Gender { get; set; }
    public string Party { get; set; }
}