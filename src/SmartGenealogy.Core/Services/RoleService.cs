

using NetTopologySuite.Index.HPRtree;

using SmartGenealogy.Data.Repositories;

namespace SmartGenealogy.Core.Services;

public class RoleService(
    RoleRepository roleRepository,
    DatabaseSettings databaseSettings)
    : IRoleService
{
    /// <summary>
    /// Retrieves a list of all roles from the database.
    /// </summary>
    /// <returns>A list of <see cref="Role"/> objects.</returns>
    public async Task<List<Role>> ListAsync()
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();
        roleRepository.Connection = connection;

        var roles = await roleRepository.ListAsync();
        return roles;
    }

    /// <summary>
    /// Retrieves a list of roles by event type from the database.
    /// </summary>
    /// <param name="eventType">Event type.</param>
    /// <returns>A list of <see cref="Role"/> objects.</returns>
    public async Task<List<Role>> ListAsync(int eventType)
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();
        roleRepository.Connection = connection;

        var roles = await roleRepository.ListAsync(eventType);
        return roles;
    }

    /// <summary>
    /// Retrieves a specific fact type by its ID.
    /// </summary>
    /// <param name="id">The ID of the role.</param>
    /// <returns>A <see cref="Role"/> object if found; otherwise, null.</returns>
    public async Task<Role?> GetRoleAsync(int id)
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();
        roleRepository.Connection = connection;

        var role = await roleRepository.GetAsync(id);
        return role;
    }

    /// <summary>
    /// Saves a role to the database. If the role id is 0, a new role is created; otherwise, the existing role is updated.
    /// </summary>
    /// <param name="item">The role to save.</param>
    /// <returns>The Id of the saved role.</returns>
    public async Task<int> SaveRoleAsync(Role item)
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();
        roleRepository.Connection = connection;

        var roleId = await roleRepository.SaveItemAsync(item);
        return roleId;
    }

    /// <summary>
    /// Deletes a role from the database.
    /// </summary>
    /// <param name="item">The role to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteRoleAsync(Role item)
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();
        roleRepository.Connection = connection;

        var roles = await roleRepository.DeleteItemAsync(item);
        return roles;
    }

    /// <summary>
    /// Deletes roles from the database by fact type Id.
    /// </summary>
    /// <param name="factTypeId">Fact type Id.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteRoleAsync(int factTypeId)
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();
        roleRepository.Connection = connection;

        var roles = await roleRepository.DeleteItemAsync(factTypeId);
        return roles;
    }
}