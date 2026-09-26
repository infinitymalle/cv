using Cv.Domain.Common;

namespace Cv.Application.Profiles;

public interface IProfileService
{
    Task<ProfileDto> GetAsync(Language language, CancellationToken cancellationToken);
}
