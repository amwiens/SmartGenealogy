namespace SmartGenealogy.Core.Services;

/// <summary>
/// Fact type service interface.
/// </summary>
public interface IFactTypeService
{
    /// <summary>
    /// Deletes a fact type.
    /// </summary>
    /// <param name="factType">Fact type</param>
    /// <returns><see langword="true"> if deleted, otherwise <see langword="false"/>.</returns>
    Task<bool> DeleteItemAsync(FactType factType);
}