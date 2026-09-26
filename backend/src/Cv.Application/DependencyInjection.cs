using Cv.Application.Courses;
using Cv.Application.Profiles;
using Cv.Application.Projects;
using Cv.Application.Timeline;
using Microsoft.Extensions.DependencyInjection;

namespace Cv.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITimelineService, TimelineService>();
        services.AddScoped<ICourseService, CourseService>();
        return services;
    }
}
