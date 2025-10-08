namespace SmartGenealogy.Data.Extensions;

/// <summary>
/// Role model extensions.
/// </summary>
public static class RoleExtensions
{
    /// <summary>
    /// Check if the role is null or new.
    /// </summary>
    /// <param name="role">Role</param>
    /// <returns><see langword="true"> if null or new, otherwise <see langword="false"/>.</returns>
    public static bool IsNullOrNew([NotNullWhen(false)] this Role? role)
    {
        return role is null || role.Id == 0;
    }
}