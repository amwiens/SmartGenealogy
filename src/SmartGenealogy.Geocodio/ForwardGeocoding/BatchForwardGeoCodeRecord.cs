﻿using System.Text.Json.Serialization;

namespace SmartGenealogy.Geocodio.ForwardGeocoding;

/// <summary>
/// JSON backer class used when deserializing responses from Geocodio.
/// </summary>
public class BatchForwardGeoCodeRecord
{
    private BatchForwardGeoCodeRecord()
    { }

    [JsonConstructor]
    public BatchForwardGeoCodeRecord(string query, ForwardGeoCodeResult response)
    {
        Query = query;
        Response = response;
    }

    public string? Query { get; set; }
    public ForwardGeoCodeResult? Response { get; set; }
}