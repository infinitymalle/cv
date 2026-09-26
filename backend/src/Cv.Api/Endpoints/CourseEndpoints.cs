using Cv.Application.Courses;
using Cv.Domain.Common;

namespace Cv.Api.Endpoints;

public static class CourseEndpoints
{
    public static IEndpointRouteBuilder MapCourseEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/courses", async (ICourseService service, Language? lang, CancellationToken ct) =>
                TypedResults.Ok(await service.GetOverviewAsync(lang ?? Language.Default, ct)))
            .WithTags("Courses")
            .WithName("GetCourses");

        return routes;
    }
}
