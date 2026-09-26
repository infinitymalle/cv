using Cv.Domain.Common;

namespace Cv.Application.Projects;

public interface IProjectService
{
    Task<IReadOnlyList<ProjectDto>> GetAllAsync(Language language, CancellationToken cancellationToken);
    Task<ProjectDto?> GetBySlugAsync(string slug, Language language, CancellationToken cancellationToken);
}
