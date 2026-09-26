using Cv.Domain.Common;

namespace Cv.Application.Courses;

public sealed class CourseService(ICourseRepository repository) : ICourseService
{
    public async Task<CourseOverviewDto> GetOverviewAsync(Language language, CancellationToken cancellationToken)
    {
        var courses = await repository.GetAllAsync(cancellationToken);

        var dtos = courses
            .OrderByDescending(c => c.CompletedOn)
            .ThenBy(c => c.Name.In(language), StringComparer.OrdinalIgnoreCase)
            .Select(c => new CourseDto(c.Name.In(language), c.Code, c.Summary?.In(language), c.Credits, c.Level, c.CompletedOn, c.Highlighted))
            .ToList();

        return new CourseOverviewDto(courses.Sum(c => c.Credits), dtos);
    }
}
