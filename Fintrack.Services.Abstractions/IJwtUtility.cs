namespace Fintrack.Services.Abstractions;

public interface IJwtUtility
{
    string GenerateAccessToken(Guid userId);
    string GenerateRefreshToken();
}
