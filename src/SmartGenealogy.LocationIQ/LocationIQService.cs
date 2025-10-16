using System.Text.Json;

using RestSharp;

using SmartGenealogy.LocationIQ.Models;

namespace SmartGenealogy.LocationIQ;

public class LocationIQService
{
    public async Task<List<FreeFormQueryResponse>?> GetFreeFormQuery(string place)
    {
        //Uri.EscapeDataString(place)
        var options = new RestClientOptions($"https://us1.locationiq.com/v1/search?q={Uri.EscapeDataString(place)}&format=json&addressdetails=1&statecode=1&normalizeaddress=1&normalizecity=1&postaladdress=1&normalizeimportance=1&namedetails=1&extratags=1&key=[add key]");
        var client = new RestClient(options);
        var request = new RestRequest("");
        request.AddHeader("accept", "application/json");
        var response = await client.GetAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var responseObject = JsonSerializer.Deserialize<List<FreeFormQueryResponse>>(response.Content!);
            return responseObject!;
        }
        else
        {
            return default!;
        }
    }
}