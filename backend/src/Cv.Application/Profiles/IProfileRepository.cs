using Cv.Domain.Profiles;

namespace Cv.Application.Profiles;

public interface IProfileRepository
{
    Task<Profile> GetAsync(CancellationToken cancellationToken);
}
