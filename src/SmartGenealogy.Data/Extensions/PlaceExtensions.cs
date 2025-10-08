namespace SmartGenealogy.Data.Extensions;

/// <summary>
/// Place model extensions.
/// </summary>
public static class PlaceExtensions
{
    /// <summary>
    /// Check if the place is null or new.
    /// </summary>
    /// <param name="place">Place</param>
    /// <returns><see langword="true"> if null or new, otherwise <see langword="false"/>.</returns>
    public static bool IsNullOrNew([NotNullWhen(false)] this Place? place)
    {
        return place is null || place.Id == 0;
    }
}