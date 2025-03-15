using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;

using SmartGenealogy.Geocodio.Enums;
using SmartGenealogy.Geocodio.Exceptions;
using SmartGenealogy.Geocodio.ForwardGeocoding;
using SmartGenealogy.Geocodio.ReverseGeocoding;

namespace SmartGenealogy.Geocodio;

public class GeoCoder
{
    private string _apiKey;
    private HttpClient _httpClient;

    private const string regularApiBase = "https://api.geocod.io/";
    private const string hippaApiBase = "https://api-hipaa.geocod.io/";
    private const string forwardGeocodeEndpoint = "geocode/";
    private const string reverseGeocodeEndpoint = "reverse/";
    public const string ClientGeocodioApiVersionPrefix = "v1.7";

    public string ClientGeocodioApiUrl { get; }
    public ApiClientType ClientType { get; }

    public GeoCoder(string apiKey, ApiClientType geocodioClientType)
    {
        _apiKey = apiKey;
        if (geocodioClientType == ApiClientType.RegularApi)
        {
            ClientGeocodioApiUrl = Path.Combine(regularApiBase, ClientGeocodioApiVersionPrefix);
            ClientType = geocodioClientType;
        }
        else
        {
            ClientGeocodioApiUrl = Path.Combine(hippaApiBase, ClientGeocodioApiVersionPrefix);
            ClientType = geocodioClientType;
        }

        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(ClientGeocodioApiUrl);
    }

    /// <summary>
    /// Method used to forward geocode a single address.
    /// </summary>
    /// <param name="addressToGeocode">The address we want to forward geocode.</param>
    /// <param name="fieldSettings">
    /// Our field settings object used to determine which additional data fields we want to query.
    /// </param>
    /// <returns>The results returned from Geocodio.</returns>
    public async Task<BatchForwardGeoCodeResult> ForwardGeocodeAsync(string addressToGeocode, GeocodioDataFieldSettings fieldSettings)
    {
        var fieldQueryString = this.PrepareDataFieldsQueryString(fieldSettings);

        var responseData = await SingleForwardGeocodeWebRequest(addressToGeocode, fieldQueryString);

        var result = JsonSerializer.Deserialize<ForwardGeoCodeResult>(responseData)!;

        // Wrap result from GeoCodio in BatchForwardGeocodeREsult because we always want to return a BatchForwardGeoCodeResult
        var record = new BatchForwardGeoCodeRecord(addressToGeocode, result);

        return new BatchForwardGeoCodeResult(new BatchForwardGeoCodeRecord[] { record });
    }

    /// <summary>
    /// Method used to batch forward geocode a bunch of addresses.
    /// </summary>
    /// <param name="inputAddresses">A list of address strings.</param>
    /// <param name="fieldSettings">
    /// Our field settings object used to determine which additional data fields we want to query.
    /// </param>
    /// <returns>The results returned from Geocodio.</returns>
    public async Task<BatchForwardGeoCodeResult> ForwardGeocodeAsync(List<string> inputAddresses, GeocodioDataFieldSettings fieldSettings)
    {
        var fieldQueryString = this.PrepareDataFieldsQueryString(fieldSettings);

        var jsonDataString = JsonSerializer.Serialize(inputAddresses);

        var responseData = await BatchForwardGeocodeWebRequest(jsonDataString, fieldQueryString);

        var results = JsonSerializer.Deserialize<BatchForwardGeoCodeResult>(responseData)!;

        return results;
    }

    private async Task<string> BatchForwardGeocodeWebRequest(string jsonDataString, string fieldQueryString)
    {
        var queryString = PrepareWebQueryString(GeocodingOperationType.BatchForward, "", fieldQueryString);

        var url = Path.Combine(forwardGeocodeEndpoint, queryString);

        var payload = new StringContent(jsonDataString, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, payload);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new GeocodingException((int)response.StatusCode);
        }

        return await response.Content.ReadAsStringAsync();
    }

    private async Task<string> SingleForwardGeocodeWebRequest(string addressToGeocode, string fieldQueryString)
    {
        var queryString = PrepareWebQueryString(GeocodingOperationType.SingleForward, addressToGeocode, fieldQueryString);

        var url = Path.Combine(forwardGeocodeEndpoint, queryString);

        var response = await _httpClient.GetAsync(url);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new GeocodingException((int)response.StatusCode);
        }

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<BatchReverseGeoCodingResult> ReverseGeocodeAsync(string latLong, GeocodioDataFieldSettings fieldSettings)
    {
        var fieldsQuery = this.PrepareDataFieldsQueryString(fieldSettings);

        var json = await SingleReverseGeocodeWebRequest(latLong, fieldsQuery);

        var result = JsonSerializer.Deserialize<ReverseGeoCodeResult>(json)!;

        var response = new BatchReverseGeoCodeResponse(latLong, result);

        return new BatchReverseGeoCodingResult(new BatchReverseGeoCodeResponse[] { response });
    }

    /// <summary>
    /// Method which handles reverse geo-coding a list of lat-long strings in decimal degrees format
    /// i.e. "48.434325, -76.434543"
    /// </summary>
    /// <param name="inputAddresses">
    /// Our list of points to reverse geo-code. Strings in decimal degrees format i.e. "48.434325, -76.434543"
    /// </param>
    /// <param name="fieldSettings">
    /// Our field settings object used to determine which additional data fields we want to query.
    /// </param>
    /// <returns>The results from Geocodio.</returns>
    public async Task<BatchReverseGeoCodingResult> ReverseGeocodeAsync(List<string> inputAddresses, GeocodioDataFieldSettings fieldSettings)
    {
        var fieldsQuery = this.PrepareDataFieldsQueryString(fieldSettings);

        var jsonPostData = JsonSerializer.Serialize(inputAddresses);

        var json = await BatchReverseGeocodeWebRequest(jsonPostData, fieldsQuery);

        var results = JsonSerializer.Deserialize<BatchReverseGeoCodingResult>(json)!;

        return results;
    }

    private async Task<string> SingleReverseGeocodeWebRequest(string latLong, string fieldQueryString)
    {
        var queryString = PrepareWebQueryString(GeocodingOperationType.SingleReverse, latLong, fieldQueryString);

        var url = Path.Combine(reverseGeocodeEndpoint, queryString);

        var response = await _httpClient.GetAsync(url);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new GeocodingException((int)response.StatusCode);
        }

        return await response.Content.ReadAsStringAsync();
    }

    private async Task<string> BatchReverseGeocodeWebRequest(string jsonPostData, string fieldQueryString)
    {
        // Pass empty string as second parameter; locations to reverse geocode are passed as payload
        // argument to HttpClient
        var queryString = PrepareWebQueryString(GeocodingOperationType.BatchReverse, "", fieldQueryString);

        var url = Path.Combine(reverseGeocodeEndpoint, queryString);

        var payload = new StringContent(jsonPostData, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, payload);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new GeocodingException((int)response.StatusCode);
        }

        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Method used to prepare the additional (and optional) field components of our query, for
    /// things like Census or School district fields.
    /// </summary>
    /// <param name="fieldSettings">
    /// Our field settings object used to determine which additional data fields we want to query.
    /// </param>
    /// <returns>A formatted string which will be appended to the URL when hitting Geocodio.</returns>
    public string PrepareDataFieldsQueryString(GeocodioDataFieldSettings fieldSettings)
    {
        var fields = new List<string>();

        foreach (var fieldKey in GeocodioDataFieldSettings.ValidGeocodioFields)
        {
            if (fieldSettings.GetFieldQueryStatus(fieldKey))
            {
                fields.Add(fieldKey);
            }
        }

        if (fields.Count == 0)
        {
            // No fields to query, give them an empty string.
            return "";
        }
        else
        {
            return "&fields=" + string.Join(",", fields);
        }
    }

    /// <summary>
    /// Prepares the mandatory parts of the URL for sending a request to Geocodio
    /// </summary>
    /// <param name="geocodingOperation">The type of Geocoding operation.</param>
    /// <param name="payload">Payload; only used when sending a single reverse geocode operation</param>
    /// <param name="dataFieldsQueryString">The fields we wish to query.</param>
    /// <returns>A string which is the base URL we will access</returns>
    public string PrepareWebQueryString(GeocodingOperationType geocodingOperation, string payload, string dataFieldsQueryString)
    {
        if (geocodingOperation == GeocodingOperationType.SingleForward)
        {
            // Single forward
            var forwardGeoCodequery = "?api_key={0}&q={1}";

            var query = HttpUtility.UrlEncode(payload);
            query = query + dataFieldsQueryString;
            query = string.Format(forwardGeoCodequery, _apiKey, query);
            return query;
        }
        else if (geocodingOperation == GeocodingOperationType.BatchForward)
        {
            // Batch forward
            var batchForwardGeoCodequery = "?api_key={0}";
            var query = string.Format(batchForwardGeoCodequery, _apiKey);
            query = query + dataFieldsQueryString;
            return query;
        }
        else if (geocodingOperation == GeocodingOperationType.SingleReverse)
        {
            // Batch reverse
            var singleReverseGeoCodequery = "?api_key={0}&q={1}";
            var queryString = string.Format(singleReverseGeoCodequery, _apiKey, payload);
            queryString = queryString + dataFieldsQueryString;
            return queryString;
        }
        else
        {
            // Batch reverse
            var batchReverseGeocodeQuery = "?api_key={0}";
            var query = string.Format(batchReverseGeocodeQuery, _apiKey);
            query = query + dataFieldsQueryString;
            return query;
        }
    }
}