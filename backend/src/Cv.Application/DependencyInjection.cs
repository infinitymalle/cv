using Cv.Application.Projects;
using Microsoft.Extensions.DependencyInjection;

namespace Cv.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectService>();
        return services;
    }
}
