using Cv.Application.Common;
using Cv.Domain.Common;

namespace Cv.Application.Profiles;

public sealed class ProfileService(IProfileRepository repository) : IProfileService
{
    public async Task<ProfileDto> GetAsync(Language language, CancellationToken cancellationToken)
    {
        var profile = await repository.GetAsync(cancellationToken);

        var links = profile.Links
            .Select(link => (Label: link.Label.In(language), Href: SafeLink.ToHref(link.Url)))
            .Where(link => link.Href is not null)
            .Select(link => new LinkDto(link.Label, link.Href!))
            .ToList();

        var skills = profile.Skills
            .Select(group => new SkillGroupDto(
                group.Category.In(language),
                group.Items.Select(item => item.In(language)).ToList()))
            .ToList();

        var languages = profile.Languages
            .Select(l => new SpokenLanguageDto(l.Name.In(language), l.Proficiency.In(language)))
            .ToList();

        return new ProfileDto(
            profile.Name,
            profile.Headline.In(language),
            profile.Summary.In(language),
            links,
            skills,
            languages);
    }
}
