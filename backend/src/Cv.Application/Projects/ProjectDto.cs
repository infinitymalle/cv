namespace Cv.Application.Projects;

/// <summary>The shape of a project as the API exposes it. Kept separate from the domain model on purpose.</summary>
public sealed record ProjectDto(
    string Slug,
    string Title,
    string Summary,
    IReadOnlyList<string> Technologies,
    DateOnly StartedOn,
    DateOnly? FinishedOn,
    string? RepositoryUrl);
