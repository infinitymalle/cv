namespace Cv.Application.Common;

public static class SafeLink
{
    /// <summary>
    /// Only absolute http(s) links reach the browser; stops e.g. "javascript:" URLs
    /// from ending up in an &lt;a href&gt;. Returns null for anything else.
    /// </summary>
    public static string? ToHref(Uri? uri) =>
        uri is { IsAbsoluteUri: true } && (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
            ? uri.AbsoluteUri
            : null;
}
