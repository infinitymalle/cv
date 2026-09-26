using System.Net;
using System.Net.Http.Json;
using Cv.Application.Projects;

namespace Cv.Api.Tests;

public class ProjectEndpointsTests(CvApiFactory factory) : IClassFixture<CvApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public async Task Get_projects_returns_all_projects_from_content()
    {
        var projects = await _client.GetFromJsonAsync<List<ProjectDto>>("/api/v1/projects", Ct);

        Assert.NotNull(projects);
        Assert.Equal(["test-project-new", "test-project-old"], projects.Select(p => p.Slug));
    }

    [Fact]
    public async Task Get_project_by_slug_returns_project()
    {
        var project = await _client.GetFromJsonAsync<ProjectDto>("/api/v1/projects/test-project-old", Ct);

        Assert.NotNull(project);
        Assert.Equal("Old test project", project.Title);
    }

    [Theory]
    [InlineData("/api/v1/projects/missing")]
    [InlineData("/api/v1/projects/..%2F..%2Fappsettings.json")]
    [InlineData("/api/v1/does-not-exist")]
    public async Task Unknown_resources_return_404(string url)
    {
        var response = await _client.GetAsync(url, Ct);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Write_methods_are_not_allowed()
    {
        var response = await _client.PostAsync("/api/v1/projects", new StringContent("{}"), Ct);

        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
    }

    [Fact]
    public async Task Responses_include_security_headers()
    {
        var response = await _client.GetAsync("/api/v1/projects", Ct);

        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").Single());
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").Single());
        Assert.True(response.Headers.Contains("Content-Security-Policy"));
        Assert.False(response.Headers.Contains("Server"));
    }
}
