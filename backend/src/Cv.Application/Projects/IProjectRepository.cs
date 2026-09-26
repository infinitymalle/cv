using Cv.Domain.Projects;

namespace Cv.Application.Projects;

/// <summary>
/// Where projects come from. The application only knows this interface;
/// Cv.Infrastructure decides whether it is a JSON file, a database, or something else.
/// </summary>
public interface IProjectRepository
{
    Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken);
}
