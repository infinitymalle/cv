using System.Diagnostics.CodeAnalysis;

namespace Cv.Domain.Common;

/// <summary>A language the CV content is available in. Adding one: add it here and to the content files.</summary>
public readonly record struct Language : IParsable<Language>
{
    public static readonly Language English = new("en");
    public static readonly Language Swedish = new("sv");

    /// <summary>Used when no language is requested, and as fallback when a translation is missing.</summary>
    public static readonly Language Default = English;

    public static IReadOnlyList<Language> Supported { get; } = [English, Swedish];

    private Language(string code) => Code = code;

    /// <summary>ISO 639-1 code, e.g. "sv".</summary>
    public string Code { get; }

    // ASP.NET Core uses TryParse to bind "?lang=sv"; unsupported values automatically become 400 Bad Request.
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Language result)
    {
        foreach (var language in Supported)
        {
            if (string.Equals(language.Code, s, StringComparison.OrdinalIgnoreCase))
            {
                result = language;
                return true;
            }
        }

        result = Default;
        return false;
    }

    public static Language Parse(string s, IFormatProvider? provider) =>
        TryParse(s, provider, out var result) ? result : throw new FormatException($"Unsupported language '{s}'.");

    public override string ToString() => Code;
}
