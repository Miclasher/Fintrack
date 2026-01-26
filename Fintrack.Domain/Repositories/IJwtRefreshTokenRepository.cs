using Fintrack.Domain.Entities;

namespace Fintrack.Domain.Repositories;

public interface IJwtRefreshTokenRepository
{
    Task InvalidateUserTokenAsync(Guid userId, CancellationToken cancellationToken = default);
    Task ReplaceUserTokenAsync(Guid userId, JwtRefreshToken newToken, CancellationToken cancellationToken = default);
    Task<JwtRefreshToken> GetTokenAsync(string token, CancellationToken cancellationToken = default);
}
