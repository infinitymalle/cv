using Cv.Application.Courses;
using Cv.Domain.Courses;

namespace Cv.Infrastructure.Content;

/// <summary>Reads completed courses from content/courses.json.</summary>
public sealed class JsonCourseRepository(JsonContentReader reader) : ICourseRepository
{
    public async Task<IReadOnlyList<Course>> GetAllAsync(CancellationToken cancellationToken) =>
        await reader.ReadAsync<List<Course>>("courses.json", cancellationToken);
}
