using Cv.Domain.Common;

namespace Cv.Domain.Tests.Common;

public class LocalizedTextTests
{
    [Fact]
    public void In_returns_the_requested_translation()
    {
        var text = new LocalizedText(new Dictionary<string, string> { ["en"] = "Developer", ["sv"] = "Utvecklare" });

        Assert.Equal("Utvecklare", text.In(Language.Swedish));
        Assert.Equal("Developer", text.In(Language.English));
    }

    [Fact]
    public void In_falls_back_to_default_language_when_translation_is_missing()
    {
        var text = new LocalizedText(new Dictionary<string, string> { ["en"] = "Developer" });

        Assert.Equal("Developer", text.In(Language.Swedish));
    }

    [Fact]
    public void Invariant_text_is_the_same_in_every_language()
    {
        var text = LocalizedText.Invariant("Luleå");

        Assert.All(Language.Supported, language => Assert.Equal("Luleå", text.In(language)));
    }

    [Fact]
    public void Default_language_translation_is_required()
    {
        Assert.Throws<ArgumentException>(() =>
            new LocalizedText(new Dictionary<string, string> { ["sv"] = "Utvecklare" }));
    }
}
