namespace Cv.Application.Projects;

public interface IProjectService
{
    Task<IReadOnlyList<ProjectDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<ProjectDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
}
