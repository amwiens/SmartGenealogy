namespace SmartGenealogy.Core.Services;

/// <summary>
/// Fact type service.
/// </summary>
/// <param name="factTypeRepository">Fact type repository</param>
/// <param name="roleRepository">Role repository</param>
/// <param name="logger">Logger</param>
public class FactTypeService(
    FactTypeRepository factTypeRepository,
    RoleRepository roleRepository,
    ILogger<FactTypeService> logger)
    : IFactTypeService
{
    /// <summary>
    /// Deletes a fact type.
    /// </summary>
    /// <param name="factType"></param>
    /// <returns><see langword="true"> if deleted, otherwise <see langword="false"/>.</returns>
    public async Task<bool> DeleteItemAsync(FactType factType)
    {
        if (factType is null)
            return false;

        // Deletes the roles associated with the fact type.
        await roleRepository.DeleteItemAsync(factType.Id);
        // Deletes the fact type
        await factTypeRepository.DeleteItemAsync(factType);

        return true;
    }
}