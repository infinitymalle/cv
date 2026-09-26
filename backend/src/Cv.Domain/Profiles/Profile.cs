using Cv.Domain.Common;

namespace Cv.Domain.Profiles;

/// <summary>Who the CV belongs to: introduction, links, skills and spoken languages.</summary>
public sealed record Profile
{
    public required string Name { get; init; }
    public required LocalizedText Headline { get; init; }
    public required LocalizedText Summary { get; init; }
    public IReadOnlyList<ProfileLink> Links { get; init; } = [];
    public IReadOnlyList<SkillGroup> Skills { get; init; } = [];
    public IReadOnlyList<SpokenLanguage> Languages { get; init; } = [];
}

public sealed record ProfileLink
{
    public required LocalizedText Label { get; init; }
    public required Uri Url { get; init; }
}

public sealed record SkillGroup
{
    public required LocalizedText Category { get; init; }
    public IReadOnlyList<LocalizedText> Items { get; init; } = [];
}

public sealed record SpokenLanguage
{
    public required LocalizedText Name { get; init; }
    public required LocalizedText Proficiency { get; init; }
}
