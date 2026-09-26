using System.Text.RegularExpressions;

namespace Cv.Domain.Projects;

/// <summary>A slug is the URL-friendly id of a project, e.g. "cv-website".</summary>
public static partial class ProjectSlug
{
    public const int MaxLength = 100;

    public static bool IsValid(string? slug) =>
        slug is { Length: > 0 and <= MaxLength } && Pattern().IsMatch(slug);

    [GeneratedRegex("^[a-z0-9]+(?:-[a-z0-9]+)*$")]
    private static partial Regex Pattern();
}
