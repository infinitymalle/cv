using Cv.Domain.Courses;

namespace Cv.Application.Courses;

public interface ICourseRepository
{
    Task<IReadOnlyList<Course>> GetAllAsync(CancellationToken cancellationToken);
}
