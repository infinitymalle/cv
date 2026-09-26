using Cv.Application.Timeline;
using Cv.Domain.Common;
using Cv.Domain.Timeline;
using static Cv.Application.Tests.TestData;

namespace Cv.Application.Tests.Timeline;

public class TimelineServiceTests
{
    private static readonly CancellationToken Ct = CancellationToken.None;

    [Fact]
    public async Task Ongoing_entries_come_first_then_most_recently_finished()
    {
        var service = CreateService(experience:
        [
            NewEntry("Finished 2020", new DateOnly(2019, 1, 1), new DateOnly(2020, 1, 1)),
            NewEntry("Ongoing", new DateOnly(2018, 1, 1), finishedOn: null),
            NewEntry("Finished 2024", new DateOnly(2023, 1, 1), new DateOnly(2024, 1, 1)),
        ]);

        var result = await service.GetExperienceAsync(Language.English, Ct);

        Assert.Equal(["Ongoing", "Finished 2024", "Finished 2020"], result.Select(e => e.Organization));
    }

    [Fact]
    public async Task Texts_are_returned_in_the_requested_language()
    {
        var entry = NewEntry("Org", new DateOnly(2020, 1, 1), null) with
        {
            Role = Text("Developer", "Utvecklare"),
            Location = Text("Gothenburg", "Göteborg"),
            Highlights = [Text("Built APIs", "Byggde API:er")],
        };
        var service = CreateService(education: [entry]);

        var result = Assert.Single(await service.GetEducationAsync(Language.Swedish, Ct));

        Assert.Equal("Utvecklare", result.Role);
        Assert.Equal("Göteborg", result.Location);
        Assert.Equal(["Byggde API:er"], result.Highlights);
    }

    private static TimelineService CreateService(TimelineEntry[]? experience = null, TimelineEntry[]? education = null) =>
        new(new FakeTimelineRepository(experience ?? [], education ?? []));

    private static TimelineEntry NewEntry(string organization, DateOnly startedOn, DateOnly? finishedOn) => new()
    {
        Organization = Text(organization),
        Role = Text("Role"),
        StartedOn = startedOn,
        FinishedOn = finishedOn,
    };

    private sealed class FakeTimelineRepository(IReadOnlyList<TimelineEntry> experience, IReadOnlyList<TimelineEntry> education)
        : ITimelineRepository
    {
        public Task<IReadOnlyList<TimelineEntry>> GetExperienceAsync(CancellationToken cancellationToken) => Task.FromResult(experience);
        public Task<IReadOnlyList<TimelineEntry>> GetEducationAsync(CancellationToken cancellationToken) => Task.FromResult(education);
    }
}
