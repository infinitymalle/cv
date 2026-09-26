using Cv.Domain.Common;

namespace Cv.Application.Timeline;

public interface ITimelineService
{
    Task<IReadOnlyList<TimelineEntryDto>> GetExperienceAsync(Language language, CancellationToken cancellationToken);
    Task<IReadOnlyList<TimelineEntryDto>> GetEducationAsync(Language language, CancellationToken cancellationToken);
}
