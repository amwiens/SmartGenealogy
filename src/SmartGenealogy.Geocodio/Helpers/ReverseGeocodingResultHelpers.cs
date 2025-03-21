﻿using SmartGenealogy.Geocodio.Models;
using SmartGenealogy.Geocodio.ReverseGeocoding;

namespace SmartGenealogy.Geocodio.Helpers;

/// <summary>
/// Helper object to transform results from Geocodio into keyed collections.
/// </summary>
public static class ReverseGeocodingResultHelpers
{
    /// <summary>
    /// Transforms batch reverse geocode results into a dictionary keyed by the lat-long that was
    /// queried. Puts duplicate requests into the same list and returns a 0 item list when no
    /// results are returned.
    /// </summary>
    /// <param name="results"></param>
    /// <returns>A dictionary of GeoCodeInfo (the results) keyed by the lat-long that was queried.</returns>
    public static Dictionary<string, List<GeoCodeInfo>> MakeReverseResultsDict(BatchReverseGeoCodingResult results)
    {
        var resultsDict = new Dictionary<string, List<GeoCodeInfo>>();

        foreach (var record in results.Results)
        {
            if (record.Response.Results.Length > 0)
            {
                if (resultsDict.ContainsKey(record.Query))
                {
                    resultsDict[record.Query].AddRange(record.Response.Results);
                }
                else
                {
                    resultsDict.Add(record.Query, new List<GeoCodeInfo>());
                    resultsDict[record.Query].AddRange(record.Response.Results);
                }
            }
            else
            {
                resultsDict.Add(record.Query, new List<GeoCodeInfo>(0));
            }
        }

        return resultsDict;
    }
}