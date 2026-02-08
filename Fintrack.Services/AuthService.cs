using Fintrack.Domain.Entities;
using Fintrack.Domain.Repositories;
using Fintrack.Services.Abstractions;
using Fintrack.Services.Utilities;
using Fintrack.Shared;
using Microsoft.Extensions.Configuration;
using System.Numerics;
using System.Security.Cryptography;

namespace Fintrack.Services;

public sealed class AuthService : IAuthService
{
    private readonly IJwtUtility _jwtProvider;
    private readonly IRepositoryManager _repositoryManager;

    private readonly int RefreshTokenLifetime;
    public AuthService(IJwtUtility jwtProvider, IRepositoryManager repositoryManager, IConfiguration configuration)
    {
        _jwtProvider = jwtProvider;
        _repositoryManager = repositoryManager;

        RefreshTokenLifetime = int.Parse(configuration["Jwt:RefreshTokenExpirationMinutes"] ?? throw new NullReferenceException("Please specify refresh token expiration time in config (Jwt:RefreshTokenExpirationMinutes)"));
    }

    public async Task<AuthResponseDTO> LoginAsync(UserLoginDTO user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        var userToLogin = await _repositoryManager.User.GetByUsernameAsync(user.Username, cancellationToken);

        if (userToLogin is null)
        {
            throw new InvalidDataException("Username not found");
        }
        else if (!HashUtility.VerifyPassword(user.Password, userToLogin.PasswordHash, userToLogin.Salt))
        {
            throw new InvalidDataException("Invalid password");
        }

        var refreshToken = GenerateJwtTokenEntity(userToLogin.Id);

        await _repositoryManager.RefreshToken.ReplaceUserTokenAsync(userToLogin.Id, refreshToken, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponseDTO
        {
            AccessToken = _jwtProvider.GenerateAccessToken(userToLogin.Id),
            RefreshToken = refreshToken.Token
        };
    }

    public async Task<AuthResponseDTO> RegisterAsync(UserRegisterDTO newUser, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(newUser, nameof(newUser));

        if (await _repositoryManager.User.UsernameExistsAsync(newUser.Username, cancellationToken))
        {
            throw new InvalidOperationException("Username is already taken");
        }

        var userToAdd = CreateNewUserEntity(newUser);

        var refreshToken = GenerateJwtTokenEntity(userToAdd.Id);

        await _repositoryManager.User.AddAsync(userToAdd, cancellationToken);

        await _repositoryManager.RefreshToken.ReplaceUserTokenAsync(userToAdd.Id, refreshToken, cancellationToken);

        await AddDefaultTransactionTypes(userToAdd.Id, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponseDTO
        {
            RefreshToken = refreshToken.Token,
            AccessToken = _jwtProvider.GenerateAccessToken(userToAdd.Id)
        };
    }

    public async Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var token = await _repositoryManager.RefreshToken.GetTokenAsync(refreshToken, cancellationToken);

        if (token is null || token.User is null || token.ExpiryDate < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Invalid token");
        }

        var newRefreshToken = GenerateJwtTokenEntity(token.UserId);

        await _repositoryManager.RefreshToken.ReplaceUserTokenAsync(token.UserId, newRefreshToken, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponseDTO
        {
            AccessToken = _jwtProvider.GenerateAccessToken(token.UserId),
            RefreshToken = newRefreshToken.Token
        };
    }

    private static User CreateNewUserEntity(UserRegisterDTO newUser)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);

        var hashedPassword = HashUtility.HashPassword(newUser.PlainPassword, saltBytes);

        var userToAdd = new User
        {
            Id = Guid.NewGuid(),
            Username = newUser.Username,
            PasswordHash = hashedPassword,
            Salt = Convert.ToBase64String(saltBytes),
            Email = newUser.Email,
            DisplayName = newUser.DisplayName,
            MonobankAccountId = newUser.MonobankApiToken
        };
        return userToAdd;
    }

    private JwtRefreshToken GenerateJwtTokenEntity(Guid userId)
    {
        return new JwtRefreshToken
        {
            Id = Guid.NewGuid(),
            Token = _jwtProvider.GenerateRefreshToken(),
            ExpiryDate = DateTime.UtcNow.AddMinutes(RefreshTokenLifetime),
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };
    }

    private async Task AddDefaultTransactionTypes(Guid userId, CancellationToken cancellationToken = default)
    {
        var templates = await _repositoryManager.TransactionTypeTemplate.GetAllWithMccAsync(cancellationToken)
            ?? throw new InvalidDataException("Cannot find transaction type templates.");

        foreach (var template in templates)
        {
            var newTransactionType = new TransactionType
            {
                UserId = userId,
                Name = template.Name,
                IsExpense = template.IsExpense,
                Mccs = template.Mccs.ToList()
            };

            await _repositoryManager.TransactionType.AddAsync(newTransactionType, cancellationToken);
        }
    }
}
