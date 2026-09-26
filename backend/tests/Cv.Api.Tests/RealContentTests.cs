using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Cv.Domain.Common;

namespace Cv.Api.Tests;

/// <summary>
/// Guards the real content/ folder, so a typo or a missing translation fails CI
/// instead of breaking the live site.
/// </summary>
public class RealContentTests(RealContentApiFactory factory) : IClassFixture<RealContentApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    public static TheoryData<string, string> EndpointsInEveryLanguage()
    {
        var data = new TheoryData<string, string>();
        foreach (var endpoint in new[] { "profile", "projects", "experience", "education", "courses" })
        {
            foreach (var language in Language.Supported)
            {
                data.Add(endpoint, language.Code);
            }
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(EndpointsInEveryLanguage))]
    public async Task Every_endpoint_loads_the_real_content(string endpoint, string lang)
    {
        var response = await _client.GetAsync($"/api/v1/{endpoint}?lang={lang}", Ct);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public void Every_translated_text_has_all_languages()
    {
        var supported = Language.Supported.Select(l => l.Code).ToHashSet();
        var problems = new List<string>();

        foreach (var file in Directory.GetFiles(RealContentApiFactory.RealContentPath, "*.json"))
        {
            var root = JsonNode.Parse(File.ReadAllText(file), documentOptions: new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            });
            FindIncompleteTranslations(root, Path.GetFileName(file), supported, problems);
        }

        Assert.True(problems.Count == 0, "Missing translations:\n" + string.Join('\n', problems));
    }

    // A translated text is an object whose keys are all language codes, e.g. { "en": "...", "sv": "..." }.
    private static void FindIncompleteTranslations(JsonNode? node, string path, HashSet<string> supported, List<string> problems)
    {
        switch (node)
        {
            case JsonObject obj when obj.Count > 0 && obj.All(p => supported.Contains(p.Key)):
                var missing = supported.Where(code => string.IsNullOrWhiteSpace(obj[code]?.GetValue<string>())).ToList();
                if (missing.Count > 0)
                {
                    problems.Add($"{path}: missing {string.Join(", ", missing)}");
                }
                break;

            case JsonObject obj:
                foreach (var (key, value) in obj)
                {
                    FindIncompleteTranslations(value, $"{path}.{key}", supported, problems);
                }
                break;

            case JsonArray array:
                for (var i = 0; i < array.Count; i++)
                {
                    FindIncompleteTranslations(array[i], $"{path}[{i}]", supported, problems);
                }
                break;
        }
    }
}
