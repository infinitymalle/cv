namespace Cv.Application.Profiles;

public sealed record ProfileDto(
    string Name,
    string Headline,
    string Summary,
    IReadOnlyList<LinkDto> Links,
    IReadOnlyList<SkillGroupDto> Skills,
    IReadOnlyList<SpokenLanguageDto> Languages);

public sealed record LinkDto(string Label, string Url);

public sealed record SkillGroupDto(string Category, IReadOnlyList<string> Items);

public sealed record SpokenLanguageDto(string Name, string Proficiency);
