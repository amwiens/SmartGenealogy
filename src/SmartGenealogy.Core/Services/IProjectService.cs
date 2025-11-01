namespace SmartGenealogy.Core.Services;

/// <summary>
/// Project service interface.
/// </summary>
public interface IProjectService
{
    /// <summary>
    /// Retrieves a list of all projects from the database.
    /// </summary>
    /// <returns>A list of <see cref="Project"/> objects.</returns>
    Task<List<Project>> ListProjectsAsync();

    /// <summary>
    /// Retrieves a specific project by its ID.
    /// </summary>
    /// <returns>A <see cref="Project"/> object if found; otherwise, null.</returns>
    Task<Project> GetProjectAsync(int id);

    /// <summary>
    /// Saves a project to the database. If the project Id is 0, a new project is created; otherwise, the existing project is updated.
    /// </summary>
    /// <param name="item">Project</param>
    /// <returns>Project Id</returns>
    Task<int> SaveProjectAsync(Project item);
}