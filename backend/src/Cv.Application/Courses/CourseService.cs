using Cv.Domain.Common;
using Cv.Domain.Courses;

namespace Cv.Application.Courses;

public sealed class CourseService(ICourseRepository repository) : ICourseService
{
    public async Task<CourseOverviewDto> GetOverviewAsync(Language language, CancellationToken cancellationToken)
    {
        var courses = await repository.GetAllAsync(cancellationToken);

        // Most relevant first: key courses, then advanced level, then basic level; alphabetical within each.
        var dtos = courses
            .OrderByDescending(c => c.Highlighted)
            .ThenByDescending(c => c.Level == CourseLevel.Advanced)
            .ThenBy(c => c.Name.In(language), StringComparer.OrdinalIgnoreCase)
            .Select(c => new CourseDto(c.Name.In(language), c.Code, c.Summary?.In(language), c.Credits, c.Level, c.Highlighted))
            .ToList();

        return new CourseOverviewDto(courses.Sum(c => c.Credits), dtos);
    }
}
