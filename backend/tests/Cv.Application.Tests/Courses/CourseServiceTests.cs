using Cv.Application.Courses;
using Cv.Domain.Common;
using Cv.Domain.Courses;
using static Cv.Application.Tests.TestData;

namespace Cv.Application.Tests.Courses;

public class CourseServiceTests
{
    private static readonly CancellationToken Ct = CancellationToken.None;

    [Fact]
    public async Task Total_credits_is_the_sum_of_all_courses()
    {
        var service = CreateService(
            NewCourse("A", credits: 7.5m),
            NewCourse("B", credits: 15m),
            NewCourse("C", credits: 7.5m));

        var overview = await service.GetOverviewAsync(Language.English, Ct);

        Assert.Equal(30m, overview.TotalCredits);
    }

    [Fact]
    public async Task Courses_are_sorted_most_recently_completed_first()
    {
        var service = CreateService(
            NewCourse("Old", completedOn: new DateOnly(2021, 1, 1)),
            NewCourse("New", completedOn: new DateOnly(2026, 1, 1)));

        var overview = await service.GetOverviewAsync(Language.English, Ct);

        Assert.Equal(["New", "Old"], overview.Courses.Select(c => c.Name));
    }

    [Fact]
    public async Task Course_names_are_returned_in_the_requested_language()
    {
        var service = CreateService(NewCourse("Real-Time Systems") with { Name = Text("Real-Time Systems", "Realtidssystem") });

        var overview = await service.GetOverviewAsync(Language.Swedish, Ct);

        Assert.Equal("Realtidssystem", Assert.Single(overview.Courses).Name);
    }

    private static CourseService CreateService(params Course[] courses) => new(new FakeCourseRepository(courses));

    private static Course NewCourse(string name, decimal credits = 7.5m, DateOnly? completedOn = null) => new()
    {
        Name = Text(name),
        Credits = credits,
        Level = CourseLevel.Basic,
        CompletedOn = completedOn ?? new DateOnly(2025, 1, 1),
    };

    private sealed class FakeCourseRepository(IReadOnlyList<Course> courses) : ICourseRepository
    {
        public Task<IReadOnlyList<Course>> GetAllAsync(CancellationToken cancellationToken) => Task.FromResult(courses);
    }
}
