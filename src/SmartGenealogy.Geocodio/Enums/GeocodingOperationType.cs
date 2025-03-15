namespace SmartGenealogy.Geocodio.Enums;

/// <summary>
/// Enum used to specify the kind of geocoding operation being performed. Used internally by the geocoder client.
/// </summary>
public enum GeocodingOperationType
{
    SingleForward,
    BatchForward,
    SingleReverse,
    BatchReverse
}