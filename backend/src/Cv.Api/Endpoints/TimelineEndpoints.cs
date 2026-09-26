using Cv.Application.Timeline;
using Cv.Domain.Common;

namespace Cv.Api.Endpoints;

public static class TimelineEndpoints
{
    public static IEndpointRouteBuilder MapTimelineEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/experience", async (ITimelineService service, Language? lang, CancellationToken ct) =>
                TypedResults.Ok(await service.GetExperienceAsync(lang ?? Language.Default, ct)))
            .WithTags("Timeline")
            .WithName("GetExperience");

        routes.MapGet("/education", async (ITimelineService service, Language? lang, CancellationToken ct) =>
                TypedResults.Ok(await service.GetEducationAsync(lang ?? Language.Default, ct)))
            .WithTags("Timeline")
            .WithName("GetEducation");

        return routes;
    }
}
