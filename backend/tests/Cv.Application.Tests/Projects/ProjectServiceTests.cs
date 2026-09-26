using Cv.Application.Projects;
using Cv.Domain.Common;
using Cv.Domain.Projects;
using static Cv.Application.Tests.TestData;

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

        var result = await service.GetAllAsync(Language.English, Ct);

        Assert.Equal(["new", "old"], result.Select(p => p.Slug));
    }

    [Fact]
    public async Task Texts_are_returned_in_the_requested_language()
    {
        var service = CreateService(NewProject("p") with { Title = Text("CV website", "CV-webbplats") });

        var swedish = await service.GetBySlugAsync("p", Language.Swedish, Ct);
        var english = await service.GetBySlugAsync("p", Language.English, Ct);

        Assert.Equal("CV-webbplats", swedish?.Title);
        Assert.Equal("CV website", english?.Title);
    }

    [Theory]
    [InlineData("does-not-exist")]
    [InlineData("../etc/passwd")]
    [InlineData("UPPERCASE")]
    [InlineData("")]
    public async Task GetBySlugAsync_returns_null_for_unknown_or_invalid_slug(string slug)
    {
        var service = CreateService(NewProject("cv-website"));

        Assert.Null(await service.GetBySlugAsync(slug, Language.English, Ct));
    }

    [Theory]
    [InlineData("https://github.com/me/repo", "https://github.com/me/repo")]
    [InlineData("javascript:alert(1)", null)]
    [InlineData("file:///C:/secret.txt", null)]
    public async Task Only_http_links_are_exposed(string url, string? expected)
    {
        var service = CreateService(NewProject("p") with { RepositoryUrl = new Uri(url) });

        var result = await service.GetBySlugAsync("p", Language.English, Ct);

        Assert.Equal(expected, result?.RepositoryUrl);
    }

    private static ProjectService CreateService(params Project[] projects) =>
        new(new FakeProjectRepository(projects));

    private static Project NewProject(string slug, DateOnly? startedOn = null) => new()
    {
        Slug = slug,
        Title = Text(slug),
        Summary = Text("summary"),
        StartedOn = startedOn ?? new DateOnly(2025, 1, 1),
    };

    /// <summary>An in-memory stand-in for the real repository, so these tests never touch files.</summary>
    private sealed class FakeProjectRepository(IReadOnlyList<Project> projects) : IProjectRepository
    {
        public Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken) =>
            Task.FromResult(projects);
    }
}
