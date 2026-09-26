using Cv.Domain.Common;
using Cv.Domain.Timeline;

namespace Cv.Application.Timeline;

public sealed class TimelineService(ITimelineRepository repository) : ITimelineService
{
    public async Task<IReadOnlyList<TimelineEntryDto>> GetExperienceAsync(Language language, CancellationToken cancellationToken) =>
        ToSortedDtos(await repository.GetExperienceAsync(cancellationToken), language);

    public async Task<IReadOnlyList<TimelineEntryDto>> GetEducationAsync(Language language, CancellationToken cancellationToken) =>
        ToSortedDtos(await repository.GetEducationAsync(cancellationToken), language);

    // Ongoing entries first, then most recently finished, then most recently started.
    private static List<TimelineEntryDto> ToSortedDtos(IEnumerable<TimelineEntry> entries, Language language) =>
        entries
            .OrderByDescending(e => e.FinishedOn ?? DateOnly.MaxValue)
            .ThenByDescending(e => e.StartedOn)
            .Select(e => new TimelineEntryDto(
                e.Organization.In(language),
                e.Role.In(language),
                e.Location?.In(language),
                e.StartedOn,
                e.FinishedOn,
                e.Highlights.Select(h => h.In(language)).ToList()))
            .ToList();
}
