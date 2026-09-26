using Cv.Domain.Timeline;

namespace Cv.Application.Timeline;

public interface ITimelineRepository
{
    Task<IReadOnlyList<TimelineEntry>> GetExperienceAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<TimelineEntry>> GetEducationAsync(CancellationToken cancellationToken);
}
