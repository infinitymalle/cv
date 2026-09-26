using Cv.Domain.Common;

namespace Cv.Domain.Courses;

/// <summary>A completed university course. Grades are intentionally not part of the model.</summary>
public sealed record Course
{
    public required LocalizedText Name { get; init; }

    /// <summary>The university's course code, e.g. "D0009E".</summary>
    public string? Code { get; init; }

    /// <summary>A short description of what the course covered.</summary>
    public LocalizedText? Summary { get; init; }

    /// <summary>Higher education credits (hp); 1 hp = 1 ECTS credit.</summary>
    public required decimal Credits { get; init; }

    public required CourseLevel Level { get; init; }
    public required DateOnly CompletedOn { get; init; }

    /// <summary>Courses especially relevant to the CV, shown first.</summary>
    public bool Highlighted { get; init; }
}

/// <summary>Swedish "grundnivå" / "avancerad nivå" (first / second cycle).</summary>
public enum CourseLevel
{
    Basic,
    Advanced,
}
