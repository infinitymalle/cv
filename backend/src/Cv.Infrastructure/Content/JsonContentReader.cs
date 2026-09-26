using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Cv.Infrastructure.Content;

/// <summary>Reads and deserializes files from the content folder. Shared by all JSON repositories.</summary>
public sealed class JsonContentReader(IOptions<ContentOptions> options)
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        // Strict parsing: typos and missing values in content files fail loudly instead of silently disappearing.
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        RespectNullableAnnotations = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        Converters =
        {
            new LocalizedTextJsonConverter(),
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false),
        },
    };

    public async Task<T> ReadAsync<T>(string fileName, CancellationToken cancellationToken)
    {
        var path = Path.Combine(options.Value.RootPath, fileName);

        await using var stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<T>(stream, SerializerOptions, cancellationToken)
            ?? throw new InvalidDataException($"Content file '{fileName}' is empty.");
    }
}
