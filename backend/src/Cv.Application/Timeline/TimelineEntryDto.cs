namespace Cv.Application.Timeline;

public sealed record TimelineEntryDto(
    string Organization,
    string Role,
    string? Location,
    DateOnly StartedOn,
    DateOnly? FinishedOn,
    IReadOnlyList<string> Highlights);
