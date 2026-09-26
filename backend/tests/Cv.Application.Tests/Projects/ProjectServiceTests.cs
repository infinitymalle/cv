using Cv.Application.Projects;
using Cv.Domain.Projects;

namespace Cv.Application.Tests.Projects;

public class ProjectServiceTests
{
    private static readonly CancellationToken Ct = CancellationToken.None;

    [Fact]
    public async Task GetAllAsync_returns_newest_project_first()
    {
        var service = CreateService(
            NewProject("old", startedOn: new DateOnly(2024, 1, 1)),
            NewProject("new", startedOn: new DateOnly(2026, 1, 1)));

        var result = await service.GetAllAsync(Ct);

        Assert.Equal(["new", "old"], result.Select(p => p.Slug));
    }

    [Fact]
    public async Task GetBySlugAsync_returns_matching_project()
    {
        var service = CreateService(NewProject("cv-website"));

        var result = await service.GetBySlugAsync("cv-website", Ct);

        Assert.NotNull(result);
        Assert.Equal("cv-website", result.Slug);
    }

    [Theory]
    [InlineData("does-not-exist")]
    [InlineData("../etc/passwd")]
    [InlineData("UPPERCASE")]
    [InlineData("")]
    public async Task GetBySlugAsync_returns_null_for_unknown_or_invalid_slug(string slug)
    {
        var service = CreateService(NewProject("cv-website"));

        Assert.Null(await service.GetBySlugAsync(slug, Ct));
    }

    [Theory]
    [InlineData("https://github.com/me/repo", "https://github.com/me/repo")]
    [InlineData("javascript:alert(1)", null)]
    [InlineData("file:///C:/secret.txt", null)]
    public async Task Only_http_links_are_exposed(string url, string? expected)
    {
        var service = CreateService(NewProject("p") with { RepositoryUrl = new Uri(url) });

        var result = await service.GetBySlugAsync("p", Ct);

        Assert.Equal(expected, result?.RepositoryUrl);
    }

    private static ProjectService CreateService(params Project[] projects) =>
        new(new FakeProjectRepository(projects));

    private static Project NewProject(string slug, DateOnly? startedOn = null) => new()
    {
        Slug = slug,
        Title = slug,
        Summary = "summary",
        StartedOn = startedOn ?? new DateOnly(2025, 1, 1),
    };

    /// <summary>An in-memory stand-in for the real repository, so these tests never touch files.</summary>
    private sealed class FakeProjectRepository(IReadOnlyList<Project> projects) : IProjectRepository
    {
        public Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken) =>
            Task.FromResult(projects);
    }
}
