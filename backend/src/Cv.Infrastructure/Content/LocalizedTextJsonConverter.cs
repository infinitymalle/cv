using System.Text.Json;
using System.Text.Json.Serialization;
using Cv.Domain.Common;

namespace Cv.Infrastructure.Content;

/// <summary>
/// Reads either a plain string (same text in every language), e.g. "Luleå",
/// or an object with one text per language, e.g. { "en": "Developer", "sv": "Utvecklare" }.
/// </summary>
public sealed class LocalizedTextJsonConverter : JsonConverter<LocalizedText>
{
    public override LocalizedText Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                return LocalizedText.Invariant(reader.GetString()!);

            case JsonTokenType.StartObject:
                var translations = JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader, options)!;
                try
                {
                    return new LocalizedText(translations);
                }
                catch (ArgumentException ex)
                {
                    throw new JsonException(ex.Message, ex);
                }

            default:
                throw new JsonException("Expected a string or an object with one text per language.");
        }
    }

    public override void Write(Utf8JsonWriter writer, LocalizedText value, JsonSerializerOptions options) =>
        throw new NotSupportedException("LocalizedText is only read from content files.");
}
