namespace SmartGenealogy.Core.Services;

/// <summary>
/// Fact type service interface.
/// </summary>
public interface IFactTypeService
{
    /// <summary>
    /// Retrieves a list of all fact types from the database.
    /// </summary>
    /// <returns>A list of <see cref="FactType"/> objects.</returns>
    Task<List<FactType>> ListAsync();

    /// <summary>
    /// Gets a fact type by identifier.
    /// </summary>
    /// <param name="id">Fact type identifier</param>
    /// <returns><see cref="FactType"/> object.</returns>
    Task<FactType?> GetFactTypeAsync(int id);

    /// <summary>
    /// Saves a fact type to the database. If the fact type Id is 0, a new fact type is created; otherwise, the existing fact type is updated.
    /// </summary>
    /// <param name="item">The fact type to save.</param>
    /// <returns>The Id of the saved fact type.</returns>
    Task<int> SaveFactTypeAsync(FactType item);

    /// <summary>
    /// Deletes a fact type.
    /// </summary>
    /// <param name="factType">Fact type</param>
    /// <returns><see langword="true"> if deleted, otherwise <see langword="false"/>.</returns>
    Task<bool> DeleteItemAsync(FactType factType);
}