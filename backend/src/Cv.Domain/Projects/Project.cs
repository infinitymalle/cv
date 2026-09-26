using Cv.Domain.Common;

namespace Cv.Domain.Projects;

/// <summary>Something built during studies or in spare time.</summary>
public sealed record Project
{
    public required string Slug { get; init; }
    public required LocalizedText Title { get; init; }
    public required LocalizedText Summary { get; init; }
    public IReadOnlyList<string> Technologies { get; init; } = [];
    public required DateOnly StartedOn { get; init; }
    public DateOnly? FinishedOn { get; init; }
    public Uri? RepositoryUrl { get; init; }
}
