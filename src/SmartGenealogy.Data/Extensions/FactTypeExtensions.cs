namespace SmartGenealogy.Data.Extensions;

/// <summary>
/// Fact Type model extensions.
/// </summary>
public static class FactTypeExtensions
{
    /// <summary>
    /// Check if the fact type is null or new.
    /// </summary>
    /// <param name="factType">Fact type.</param>
    /// <returns><see langword="true"> if null or new, otherwise <see langword="false"/>.</returns>
    public static bool IsNullOrNew([NotNullWhen(false)] this FactType? factType)
    {
        return factType is null || factType.Id == 0;
    }
}