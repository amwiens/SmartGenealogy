namespace SmartGenealogy.Core.Services;

public interface IRoleService
{
    /// <summary>
    /// Retrieves a list of all roles from the database.
    /// </summary>
    /// <returns>A list of <see cref="Role"/> objects.</returns>
    Task<List<Role>> ListAsync();

    /// <summary>
    /// Retrieves a list of roles by event type from the database.
    /// </summary>
    /// <param name="eventType">Event type.</param>
    /// <returns>A list of <see cref="Role"/> objects.</returns>
    Task<List<Role>> ListAsync(int eventType);

    /// <summary>
    /// Retrieves a specific fact type by its ID.
    /// </summary>
    /// <param name="id">The ID of the role.</param>
    /// <returns>A <see cref="Role"/> object if found; otherwise, null.</returns>
    Task<Role?> GetRoleAsync(int id);

    /// <summary>
    /// Saves a role to the database. If the role id is 0, a new role is created; otherwise, the existing role is updated.
    /// </summary>
    /// <param name="item">The role to save.</param>
    /// <returns>The Id of the saved role.</returns>
    Task<int> SaveRoleAsync(Role item);

    /// <summary>
    /// Deletes a role from the database.
    /// </summary>
    /// <param name="item">The role to delete.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> DeleteRoleAsync(Role item);

    /// <summary>
    /// Deletes roles from the database by fact type Id.
    /// </summary>
    /// <param name="factTypeId">Fact type Id.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> DeleteRoleAsync(int factTypeId);
}