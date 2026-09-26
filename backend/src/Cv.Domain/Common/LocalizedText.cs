namespace Cv.Domain.Common;

/// <summary>A text with one translation per language, e.g. { "en": "Developer", "sv": "Utvecklare" }.</summary>
public sealed class LocalizedText
{
    private readonly Dictionary<string, string> _translations;

    public LocalizedText(IReadOnlyDictionary<string, string> translations)
    {
        _translations = new Dictionary<string, string>(translations, StringComparer.OrdinalIgnoreCase);

        if (!_translations.TryGetValue(Language.Default.Code, out var fallback) || string.IsNullOrWhiteSpace(fallback))
        {
            throw new ArgumentException(
                $"A '{Language.Default.Code}' translation is required; it is the fallback for other languages.",
                nameof(translations));
        }
    }

    /// <summary>The same text in every language, e.g. names of companies or technologies.</summary>
    public static LocalizedText Invariant(string text) =>
        new(new Dictionary<string, string> { [Language.Default.Code] = text });

    /// <summary>The text in <paramref name="language"/>, or the default language if it isn't translated.</summary>
    public string In(Language language) =>
        _translations.TryGetValue(language.Code, out var text) && !string.IsNullOrWhiteSpace(text)
            ? text
            : _translations[Language.Default.Code];
}
