using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Cv.Application.Courses;
using Cv.Application.Profiles;
using Cv.Application.Timeline;

namespace Cv.Api.Tests;

public class ContentEndpointsTests(CvApiFactory factory) : IClassFixture<CvApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public async Task Profile_is_localized()
    {
        var english = await _client.GetFromJsonAsync<ProfileDto>("/api/v1/profile", Ct);
        var swedish = await _client.GetFromJsonAsync<ProfileDto>("/api/v1/profile?lang=sv", Ct);

        Assert.Equal("Test Person", english?.Name);
        Assert.Equal("Student", english?.Headline);
        Assert.Equal("Studerande", swedish?.Headline);
        Assert.Equal(["C#", "Python"], swedish?.Skills.Single().Items);
    }

    [Fact]
    public async Task Experience_and_education_are_returned()
    {
        var experience = await _client.GetFromJsonAsync<List<TimelineEntryDto>>("/api/v1/experience?lang=sv", Ct);
        var education = await _client.GetFromJsonAsync<List<TimelineEntryDto>>("/api/v1/education?lang=sv", Ct);

        var job = Assert.Single(experience!);
        Assert.Equal("Utvecklare", job.Role);
        Assert.Equal("Göteborg", job.Location);
        Assert.Null(Assert.Single(education!).FinishedOn);
    }

    [Fact]
    public async Task Courses_include_total_credits_and_level_as_text()
    {
        var json = await _client.GetFromJsonAsync<JsonObject>("/api/v1/courses", Ct);

        Assert.Equal(22.5m, json!["totalCredits"]!.GetValue<decimal>());
        Assert.Equal("advanced", json["courses"]![0]!["level"]!.GetValue<string>());
        Assert.Null(json["courses"]![0]!["grade"]); // grades are never exposed
    }

    [Fact]
    public async Task Courses_are_localized()
    {
        var overview = await _client.GetFromJsonAsync<JsonObject>("/api/v1/courses?lang=sv", Ct);

        Assert.Equal("Realtidssystem", overview!["courses"]![0]!["name"]!.GetValue<string>());
    }
}
