using Cv.Domain.Common;

namespace Cv.Application.Tests;

internal static class TestData
{
    public static LocalizedText Text(string en, string? sv = null) =>
        new(sv is null
            ? new Dictionary<string, string> { ["en"] = en }
            : new Dictionary<string, string> { ["en"] = en, ["sv"] = sv });
}
