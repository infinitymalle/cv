using Cv.Application.Profiles;
using Cv.Domain.Common;

namespace Cv.Api.Endpoints;

public static class ProfileEndpoints
{
    public static IEndpointRouteBuilder MapProfileEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/profile", async (IProfileService service, Language? lang, CancellationToken ct) =>
                TypedResults.Ok(await service.GetAsync(lang ?? Language.Default, ct)))
            .WithTags("Profile")
            .WithName("GetProfile");

        return routes;
    }
}
