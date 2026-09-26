using Cv.Application.Profiles;
using Cv.Domain.Common;
using Cv.Domain.Profiles;
using static Cv.Application.Tests.TestData;

namespace Cv.Application.Tests.Profiles;

public class ProfileServiceTests
{
    private static readonly CancellationToken Ct = CancellationToken.None;

    [Fact]
    public async Task Profile_is_returned_in_the_requested_language()
    {
        var service = new ProfileService(new FakeProfileRepository(NewProfile()));

        var profile = await service.GetAsync(Language.Swedish, Ct);

        Assert.Equal("Student", profile.Headline); // no Swedish translation: falls back to English
        Assert.Equal("Programmeringsspråk", Assert.Single(profile.Skills).Category);
        Assert.Equal("Svenska", Assert.Single(profile.Languages).Name);
    }

    [Fact]
    public async Task Unsafe_links_are_removed()
    {
        var service = new ProfileService(new FakeProfileRepository(NewProfile() with
        {
            Links =
            [
                new ProfileLink { Label = Text("GitHub"), Url = new Uri("https://github.com/me") },
                new ProfileLink { Label = Text("Evil"), Url = new Uri("javascript:alert(1)") },
            ],
        }));

        var profile = await service.GetAsync(Language.English, Ct);

        Assert.Equal("GitHub", Assert.Single(profile.Links).Label);
    }

    private static Profile NewProfile() => new()
    {
        Name = "Test Person",
        Headline = Text("Student"),
        Summary = Text("Summary", "Sammanfattning"),
        Skills = [new SkillGroup { Category = Text("Programming languages", "Programmeringsspråk"), Items = [Text("C#")] }],
        Languages = [new SpokenLanguage { Name = Text("Swedish", "Svenska"), Proficiency = Text("Native", "Modersmål") }],
    };

    private sealed class FakeProfileRepository(Profile profile) : IProfileRepository
    {
        public Task<Profile> GetAsync(CancellationToken cancellationToken) => Task.FromResult(profile);
    }
}
