using Cv.Application.Timeline;
using Cv.Domain.Timeline;

namespace Cv.Infrastructure.Content;

/// <summary>Reads content/experience.json and content/education.json.</summary>
public sealed class JsonTimelineRepository(JsonContentReader reader) : ITimelineRepository
{
    public async Task<IReadOnlyList<TimelineEntry>> GetExperienceAsync(CancellationToken cancellationToken) =>
        await reader.ReadAsync<List<TimelineEntry>>("experience.json", cancellationToken);

    public async Task<IReadOnlyList<TimelineEntry>> GetEducationAsync(CancellationToken cancellationToken) =>
        await reader.ReadAsync<List<TimelineEntry>>("education.json", cancellationToken);
}
