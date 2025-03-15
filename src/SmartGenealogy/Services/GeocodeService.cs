using SmartGenealogy.Extensions;
using SmartGenealogy.Geocodio;
using SmartGenealogy.Models;

namespace SmartGenealogy.Services;

public class GeocodeService
{
    private static string geocodioApiKey = "<insert API key here>";

    public async Task<GeocodedPlace> GetPlaceAsync(string place)
    {
        var result = new GeocodedPlace();

        var geoCoder = new GeoCoder(geocodioApiKey, ApiClientType.RegularApi);
        var fields = GeocodioDataFieldSettings.CreateDataFieldSettings();
        try
        {
            var data = await geoCoder.ForwardGeocodeAsync(place, fields);
            var record = data.Results;
            if (record != null)
            {
                var newPlace = record!.FirstOrDefault()!.Response!.Results!.FirstOrDefault()!;

                result.Country = newPlace.AddressComponents.Country.ConvertCountry();
                result.City = newPlace.AddressComponents.City;
                result.State = newPlace.AddressComponents.State.ConvertState();
                result.County = newPlace.AddressComponents.County;
                result.Latitude = newPlace.Location.Latitude;
                result.Longitude = newPlace.Location.Longitude;
            }
        }
        catch { }

        return result;
    }
}