namespace SmartGenealogy.Core.Mappers;

public static class ProjectMapper
{
    /// <summary>
    /// Map ProjectDTO to Project model.
    /// </summary>
    /// <param name="dto"><see cref="ProjectDTO"/> object.</param>
    /// <returns><see cref="Project"/> model</returns>
    public static Project ToEntity(this ProjectDTO dto)
    {
        return new Project
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Priority = dto.Priority,
            Status = dto.Status,
            Category = dto.Category,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            DateAdded = dto.DateAdded,
            DateChanged = dto.DateChanged
        };
    }

    /// <summary>
    /// Map Project model to ProjectDTO.
    /// </summary>
    /// <param name="entity"><see cref="Project"/> object.</param>
    /// <returns><see cref="ProjectDTO"/> object.</returns>
    public static ProjectDTO ToDTO(this Project entity)
    {
        return new ProjectDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Priority = entity.Priority,
            Status = entity.Status,
            Category = entity.Category,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            DateAdded = entity.DateAdded,
            DateChanged = entity.DateChanged
        };
    }
}