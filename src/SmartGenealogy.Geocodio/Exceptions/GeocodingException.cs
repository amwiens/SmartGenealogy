namespace SmartGenealogy.Geocodio.Exceptions;

/// <summary>
/// Custom exception for bubbling up HTTP status codes other than 200 OK to help determine what
/// might be wrong with our request to Geocodio.
/// </summary>
public class GeocodingException : Exception
{
    private int _returnStatusCode;

    public GeocodingException(int returnStatusCode)
    {
        _returnStatusCode = returnStatusCode;
    }

    public string GeocodioErrorMessage
    {
        get
        {
            if (_returnStatusCode == 403)
                return "403 Forbidden. Probably an invalid API key.";
            else if (_returnStatusCode == 422)
                return "422 Unprocessable Entry, i.e. an invalid address";
            else if (_returnStatusCode == 500)
                return "500 Internal Server Error. Geocodio is having issues.";
            else
                return string.Format($"Other Status Code: {_returnStatusCode}. Please let Geocodio know about these if you see them.");
        }
    }
}