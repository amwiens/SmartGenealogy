namespace SmartGenealogy.Core.Services;

/// <summary>
/// Fact type service.
/// </summary>
/// <param name="factTypeRepository">Fact type repository</param>
/// <param name="roleRepository">Role repository</param>
/// <param name="databaseSettings">Database settings</param>
/// <param name="logger">Logger</param>
public class FactTypeService(
    FactTypeRepository factTypeRepository,
    RoleRepository roleRepository,
    DatabaseSettings databaseSettings,
    ILogger<FactTypeService> logger)
    : IFactTypeService
{
    /// <summary>
    /// Retrieves a list of all fact types from the database.
    /// </summary>
    /// <returns>A list of <see cref="FactType"/> objects.</returns>
    public async Task<List<FactType>> ListAsync()
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        factTypeRepository.Connection = connection;

        var factTypes = await factTypeRepository.ListAsync();
        return factTypes;
    }

    /// <summary>
    /// Gets a fact type by identifier.
    /// </summary>
    /// <param name="id">Fact type identifier</param>
    /// <returns><see cref="FactType"/> object.</returns>
    public async Task<FactType?> GetFactTypeAsync(int id)
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        factTypeRepository.Connection = connection;

        var factType = await factTypeRepository.GetAsync(id);
        return factType;
    }

    /// <summary>
    /// Saves a fact type to the database. If the fact type Id is 0, a new fact type is created; otherwise, the existing fact type is updated.
    /// </summary>
    /// <param name="item">The fact type to save.</param>
    /// <returns>The Id of the saved fact type.</returns>
    public async Task<int> SaveFactTypeAsync(FactType item)
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        factTypeRepository.Connection = connection;

        var factTypeId = await factTypeRepository.SaveItemAsync(item);
        return factTypeId;

    }


    /// <summary>
    /// Deletes a fact type.
    /// </summary>
    /// <param name="factType"></param>
    /// <returns><see langword="true"> if deleted, otherwise <see langword="false"/>.</returns>
    public async Task<bool> DeleteItemAsync(FactType factType)
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();

        factTypeRepository.Connection = connection;
        roleRepository.Connection = connection;

        if (factType is null)
            return false;

        // Deletes the roles associated with the fact type.
        await roleRepository.DeleteItemAsync(factType.Id);
        // Deletes the fact type
        await factTypeRepository.DeleteItemAsync(factType);

        return true;
    }
}