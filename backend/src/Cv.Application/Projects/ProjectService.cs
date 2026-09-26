using Cv.Domain.Projects;

namespace Cv.Application.Projects;

public sealed class ProjectService(IProjectRepository repository) : IProjectService
{
    public async Task<IReadOnlyList<ProjectDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var projects = await repository.GetAllAsync(cancellationToken);

        return projects
            .OrderByDescending(p => p.StartedOn)
            .ThenBy(p => p.Title, StringComparer.OrdinalIgnoreCase)
            .Select(ToDto)
            .ToList();
    }

    public async Task<ProjectDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        if (!ProjectSlug.IsValid(slug))
        {
            return null;
        }

        var projects = await repository.GetAllAsync(cancellationToken);
        var project = projects.FirstOrDefault(p => p.Slug == slug);

        return project is null ? null : ToDto(project);
    }

    private static ProjectDto ToDto(Project project) => new(
        project.Slug,
        project.Title,
        project.Summary,
        project.Technologies,
        project.StartedOn,
        project.FinishedOn,
        IsSafeLink(project.RepositoryUrl) ? project.RepositoryUrl!.AbsoluteUri : null);

    // Only http(s) links reach the browser; stops e.g. "javascript:" URLs from ending up in an <a href>.
    private static bool IsSafeLink(Uri? uri) =>
        uri is { IsAbsoluteUri: true } && (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp);
}
