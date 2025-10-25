namespace SmartGenealogy.Data.Extensions;

/// <summary>
/// Source model extensions.
/// </summary>
public static class SourceExtensions
{
    /// <summary>
    /// Check if the source is null or new.
    /// </summary>
    /// <param name="source">Source</param>
    /// <returns><see langword="true"> if null or new, otherwise <see langword="false"/>.</returns>
    public static bool IsNullOrNew([NotNullWhen(false)] this Source? source)
    {
        return source is null || source.Id == 0;
    }
}