using Cv.Application.Courses;
using Cv.Application.Profiles;
using Cv.Application.Projects;
using Cv.Application.Timeline;
using Cv.Infrastructure.Content;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cv.Infrastructure;

public static class DependencyInjection
{
    /// <param name="basePath">Relative content paths in configuration are resolved against this folder.</param>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration, string basePath)
    {
        services.AddOptions<ContentOptions>()
            .Bind(configuration.GetSection(ContentOptions.SectionName))
            .ValidateDataAnnotations()
            .PostConfigure(o => o.RootPath = Path.GetFullPath(o.RootPath, basePath))
            .Validate(o => Directory.Exists(o.RootPath), "Content:RootPath must point to an existing folder.")
            .ValidateOnStart();

        services.AddSingleton<JsonContentReader>();

        // Where each kind of content comes from. Moving one to a database = change its line here.
        services.AddScoped<IProfileRepository, JsonProfileRepository>();
        services.AddScoped<IProjectRepository, JsonProjectRepository>();
        services.AddScoped<ITimelineRepository, JsonTimelineRepository>();
        services.AddScoped<ICourseRepository, JsonCourseRepository>();

        return services;
    }
}
