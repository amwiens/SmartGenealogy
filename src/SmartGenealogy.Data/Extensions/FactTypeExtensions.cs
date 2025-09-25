namespace SmartGenealogy.Data.Extensions;

/// <summary>
/// Fact Type model extensions.
/// </summary>
public static class FactTypeExtensions
{
    /// <summary>
    /// Check if the fact type is null or new.
    /// </summary>
    /// <param name="factType"></param>
    /// <returns></returns>
    public static bool IsNullOrNew([NotNullWhen(false)] this FactType? factType)
    {
        return factType is null || factType.Id == 0;
    }
}