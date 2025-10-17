namespace SmartGenealogy.Data.Extensions;

/// <summary>
/// Web link model extensions.
/// </summary>
public static class WebLinkExtensions
{
    /// <summary>
    /// Check if the web link is null or new.
    /// </summary>
    /// <param name="webLink">Web link</param>
    /// <returns><see langword="true"> if null or new, otherwise <see langword="false"/>.</returns>
    public static bool IsNullOrNew([NotNullWhen(false)] this WebLink? webLink)
    {
        return webLink is null || webLink.Id == 0;
    }
}