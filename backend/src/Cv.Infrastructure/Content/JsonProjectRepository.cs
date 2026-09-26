using Cv.Application.Projects;
using Cv.Domain.Projects;

namespace Cv.Infrastructure.Content;

/// <summary>Reads projects from content/projects.json. Swap for a database-backed repository later.</summary>
public sealed class JsonProjectRepository(JsonContentReader reader) : IProjectRepository
{
    public async Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken) =>
        await reader.ReadAsync<List<Project>>("projects.json", cancellationToken);
}
