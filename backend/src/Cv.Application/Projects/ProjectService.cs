using Cv.Application.Common;
using Cv.Domain.Common;
using Cv.Domain.Projects;

namespace Cv.Application.Projects;

public sealed class ProjectService(IProjectRepository repository) : IProjectService
{
    public async Task<IReadOnlyList<ProjectDto>> GetAllAsync(Language language, CancellationToken cancellationToken)
    {
        var projects = await repository.GetAllAsync(cancellationToken);

        return projects
            .OrderByDescending(p => p.StartedOn)
            .ThenBy(p => p.Slug, StringComparer.Ordinal)
            .Select(p => ToDto(p, language))
            .ToList();
    }

    public async Task<ProjectDto?> GetBySlugAsync(string slug, Language language, CancellationToken cancellationToken)
    {
        if (!ProjectSlug.IsValid(slug))
        {
            return null;
        }

        var projects = await repository.GetAllAsync(cancellationToken);
        var project = projects.FirstOrDefault(p => p.Slug == slug);

        return project is null ? null : ToDto(project, language);
    }

    private static ProjectDto ToDto(Project project, Language language) => new(
        project.Slug,
        project.Title.In(language),
        project.Summary.In(language),
        project.Technologies,
        project.StartedOn,
        project.FinishedOn,
        SafeLink.ToHref(project.RepositoryUrl));
}
