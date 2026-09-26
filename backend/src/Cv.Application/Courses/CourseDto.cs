using Cv.Domain.Courses;

namespace Cv.Application.Courses;

public sealed record CourseOverviewDto(decimal TotalCredits, IReadOnlyList<CourseDto> Courses);

public sealed record CourseDto(
    string Name,
    string? Code,
    string? Summary,
    decimal Credits,
    CourseLevel Level,
    bool Highlighted);
