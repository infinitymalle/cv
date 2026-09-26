using Cv.Application.Projects;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Cv.Api.Endpoints;

public static class ProjectEndpoints
{
    public static IEndpointRouteBuilder MapProjectEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/projects").WithTags("Projects");

        group.MapGet("/", async (IProjectService service, CancellationToken ct) =>
                TypedResults.Ok(await service.GetAllAsync(ct)))
            .WithName("GetProjects");

        group.MapGet("/{slug}", async Task<Results<Ok<ProjectDto>, NotFound>> (
                string slug, IProjectService service, CancellationToken ct) =>
                await service.GetBySlugAsync(slug, ct) is { } project
                    ? TypedResults.Ok(project)
                    : TypedResults.NotFound())
            .WithName("GetProjectBySlug");

        return routes;
    }
}
