namespace SmartGenealogy.Core.Services;

public class ProjectService(
    ProjectRepository projectRepository,
    DatabaseSettings databaseSettings,
    IAlertService alertService)
    : IProjectService
{
    /// <summary>
    /// Retrieves a list of all projects from the database.
    /// </summary>
    /// <returns>A list of <see cref="Project"/> objects.</returns>
    public async Task<List<Project>> ListProjectsAsync()
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();
        projectRepository.Connection = connection;

        var projectDTOs = await projectRepository.ListAsync();
        var projects = projectDTOs.Select(dto => dto.ToEntity()).ToList();
        return projects;
    }

    /// <summary>
    /// Retrieves a specific project by its ID.
    /// </summary>
    /// <returns>A <see cref="ProjectDTO"/> objectif found; otherwise, null.</returns>
    public async Task<Project> GetProjectAsync(int id)
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();
        projectRepository.Connection = connection;

        var project = await projectRepository.GetAsync(id);
        return project!.ToEntity();
    }

    /// <summary>
    /// Saves a project to the database. If the project Id is 0, a new project is created; otherwise, the existing project is updated.
    /// </summary>
    /// <param name="item">Project</param>
    /// <returns>Project Id</returns>
    public async Task<int> SaveProjectAsync(Project item)
    {
        await using var connection = new SqliteConnection(databaseSettings.ConnectionString);
        await connection.OpenAsync();
        projectRepository.Connection = connection;

        var projectId = await projectRepository.SaveItemAsync(item.ToDTO());
        return projectId;
    }
}