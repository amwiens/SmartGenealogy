namespace SmartGenealogy.Data.Extensions;

/// <summary>
/// Project model extensions.
/// </summary>
public static class ProjectExtensions
{
    /// <summary>
    /// Check if the project is null or new.
    /// </summary>
    /// <param name="project">Project</param>
    /// <returns><see langword="true"> if null or new, otherwise <see langword="false"/>.</returns>
    public static bool IsNullOrNew([NotNullWhen(false)] this Project? project)
    {
        return project is null || project.Id == 0;
    }
}