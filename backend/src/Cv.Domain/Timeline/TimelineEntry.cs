using Cv.Domain.Common;

namespace Cv.Domain.Timeline;

/// <summary>A period at an organization: a job, or a school/programme.</summary>
public sealed record TimelineEntry
{
    public required LocalizedText Organization { get; init; }
    public required LocalizedText Role { get; init; }
    public LocalizedText? Location { get; init; }
    public required DateOnly StartedOn { get; init; }

    /// <summary>Null while ongoing.</summary>
    public DateOnly? FinishedOn { get; init; }

    public IReadOnlyList<LocalizedText> Highlights { get; init; } = [];
}
