using Cv.Domain.Common;

namespace Cv.Application.Courses;

public interface ICourseService
{
    Task<CourseOverviewDto> GetOverviewAsync(Language language, CancellationToken cancellationToken);
}
