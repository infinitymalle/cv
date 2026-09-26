namespace Cv.Api.Security;

/// <summary>Bound from the "Security" configuration section.</summary>
public sealed class SecurityOptions
{
    public const string SectionName = "Security";

    /// <summary>Max requests per client IP within <see cref="RateLimitWindowSeconds"/>.</summary>
    public int RateLimitPermits { get; set; } = 100;

    public int RateLimitWindowSeconds { get; set; } = 60;

    /// <summary>
    /// Networks (CIDR, e.g. "172.28.0.0/24") of reverse proxies allowed to set X-Forwarded-For.
    /// Only trusted proxies may tell us the real client IP; otherwise anyone could fake it.
    /// </summary>
    public string[] TrustedProxyNetworks { get; set; } = [];
}
