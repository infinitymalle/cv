using Cv.Application.Profiles;
using Cv.Domain.Profiles;

namespace Cv.Infrastructure.Content;

/// <summary>Reads the profile from content/profile.json.</summary>
public sealed class JsonProfileRepository(JsonContentReader reader) : IProfileRepository
{
    public Task<Profile> GetAsync(CancellationToken cancellationToken) =>
        reader.ReadAsync<Profile>("profile.json", cancellationToken);
}
