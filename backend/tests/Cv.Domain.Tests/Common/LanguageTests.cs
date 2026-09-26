using Cv.Domain.Common;

namespace Cv.Domain.Tests.Common;

public class LanguageTests
{
    [Theory]
    [InlineData("sv", "sv")]
    [InlineData("en", "en")]
    [InlineData("SV", "sv")]
    public void TryParse_accepts_supported_codes(string input, string expectedCode)
    {
        Assert.True(Language.TryParse(input, null, out var language));
        Assert.Equal(expectedCode, language.Code);
    }

    [Theory]
    [InlineData("de")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("sv-SE")]
    public void TryParse_rejects_unsupported_codes(string? input)
    {
        Assert.False(Language.TryParse(input, null, out _));
    }
}
