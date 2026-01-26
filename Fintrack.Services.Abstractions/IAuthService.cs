using Fintrack.Shared;

namespace Fintrack.Services.Abstractions;

public interface IAuthService
{
    Task<AuthResponseDTO> RegisterAsync(UserRegisterDTO newUser, CancellationToken cancellationToken = default);
    Task<AuthResponseDTO> LoginAsync(UserLoginDTO user, CancellationToken cancellationToken = default);
    Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
