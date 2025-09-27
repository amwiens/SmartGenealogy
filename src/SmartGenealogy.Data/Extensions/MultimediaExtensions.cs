namespace SmartGenealogy.Data.Extensions;

/// <summary>
/// Multimedia model extensions.
/// </summary>
public static class MultimediaExtensions
{
    /// <summary>
    /// Check if the multimedia is null or new.
    /// </summary>
    /// <param name="multimedia"></param>
    /// <returns></returns>
    public static bool IsNullOrNew([NotNullWhen(false)] this Multimedia? multimedia)
    {
        return multimedia is null || multimedia.Id == 0;
    }
}