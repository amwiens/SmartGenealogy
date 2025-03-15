using SmartGenealogy.Geocodio.ForwardGeocoding;
using SmartGenealogy.Geocodio.Models;

namespace SmartGenealogy.Geocodio.Helpers;

/// <summary>
/// Helper object to transform results from Geocodio into keyed collections.
/// </summary>
public static class ForwardGeocodingResultHelpers
{
    /// <summary>
    /// Transforms batch forward geocode results into a dictionary keyed by the address that was queried. Puts duplicate requests
    /// into the same list and returns a 0 item list when no results are returned.
    /// </summary>
    /// <param name="results"></param>
    /// <returns>A dictionary of GeoCodeInfo (the results) keyed by the address that was queried.</returns>
    public static Dictionary<string, List<GeoCodeInfo>> MakeForwardResultsDict(BatchForwardGeoCodeResult results)
    {
        var resultsDict = new Dictionary<string, List<GeoCodeInfo>>();

        foreach (var record in results.Results)
        {
            // Make sure we have something to use...
            if (record.Response!.Results!.Length > 0)
            {
                if (resultsDict.ContainsKey(record.Query!))
                {
                    // Geocoded same place twice in same request to Geocodio
                    resultsDict[record.Query!].AddRange(record.Response.Results);
                }
                else
                {
                    resultsDict.Add(record.Query!, new List<GeoCodeInfo>());
                    resultsDict[record.Query!].AddRange(record.Response.Results);
                }
            }
            else
            {
                resultsDict.Add(record.Query!, new List<GeoCodeInfo>(0));
            }
        }

        return resultsDict;
    }
}