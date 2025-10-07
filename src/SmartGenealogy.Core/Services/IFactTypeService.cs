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
    Task DeleteItemAsync(FactType factType);
}