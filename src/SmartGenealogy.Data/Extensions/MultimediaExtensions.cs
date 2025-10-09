namespace SmartGenealogy.Data.Extensions;

/// <summary>
/// Multimedia extensions
/// </summary>
public static class MultimediaExtensions
{
    /// <summary>
    /// Check if the multimedia is null or new.
    /// </summary>
    /// <param name="multimedia">Multimedia</param>
    /// <returns><see langword="true"> if null or new, otherwise <see langword="false"/>.</returns>
    public static bool IsNullOrNew([NotNullWhen(false)] this Multimedia? multimedia)
    {
        return multimedia is null || multimedia.Id == 0;
    }
}