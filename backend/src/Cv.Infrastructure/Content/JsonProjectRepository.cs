using System.Text.Json;
using Cv.Application.Projects;
using Cv.Domain.Projects;
using Microsoft.Extensions.Options;

namespace Cv.Infrastructure.Content;

/// <summary>Reads projects from content/projects.json. Swap for a database-backed repository later.</summary>
public sealed class JsonProjectRepository(IOptions<ContentOptions> options) : IProjectRepository
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly string _filePath = Path.Combine(options.Value.RootPath, "projects.json");

    public async Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(_filePath);
        var projects = await JsonSerializer.DeserializeAsync<List<Project>>(stream, SerializerOptions, cancellationToken);
        return projects ?? [];
    }
}
