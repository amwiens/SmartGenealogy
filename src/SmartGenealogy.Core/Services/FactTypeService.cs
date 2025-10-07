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
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task DeleteItemAsync(FactType factType)
    {
        try
        {
            // Deletes the roles associated with the fact type.
            await roleRepository.DeleteItemAsync(factType.Id);
            // Deletes the fact type
            await factTypeRepository.DeleteItemAsync(factType);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting fact type");
            throw;
        }
    }
}