using Fintrack.Frontend.Services.Abstractions;
using Fintrack.Frontend.Utilities;
using Fintrack.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;

namespace Fintrack.Frontend.Services;

internal sealed class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly AuthenticationStateProvider _authStateProvider;

    private const string RefreshTokenKey = "refreshToken";
    private const string AccessTokenKey = "accessToken";

    public AuthService(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authStateProvider, IJSRuntime jsRuntime)
    {
        _httpClient = httpClientFactory.CreateClient("FintrackAPI");
        _authStateProvider = authStateProvider;
        _jsRuntime = jsRuntime;
    }

    public async Task LoginAsync(UserLoginDTO userLogin)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/login", userLogin);

        await response.CustomEnsureSuccessStatusCode();

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();

        if (authResponse is not null)
        {
            await SaveTokens(authResponse);
            (_authStateProvider as CustomAuthenticationStateProvider)!.NotifyUserLogin(authResponse.AccessToken);
        }
        else
        {
            throw new InvalidDataException("Failed to get auth response from server response.");
        }
    }

    public async Task RegisterAsync(UserRegisterDTO userRegister)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/register", userRegister);

        await response.CustomEnsureSuccessStatusCode();

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();

        if (authResponse is not null)
        {
            await SaveTokens(authResponse);
            (_authStateProvider as CustomAuthenticationStateProvider)!.NotifyUserLogin(authResponse.AccessToken);
        }
        else
        {
            throw new InvalidDataException("Failed to get auth response from server response.");
        }
    }

    public async Task<string> RefreshTokenAsync()
    {
        var refreshToken = await GetRefreshToken();

        if (string.IsNullOrEmpty(refreshToken))
        {
            await LogoutAsync();
            throw new UnauthorizedAccessException("Refresh token is missing.");
        }

        var response = await _httpClient.PostAsJsonAsync("api/Auth/loginWithRefreshToken", refreshToken);

        if (!response.IsSuccessStatusCode)
        {
            await LogoutAsync();
            throw new HttpRequestException("Failed to refresh token.");
        }

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();

        if (authResponse is null)
        {
            await LogoutAsync();
            throw new InvalidDataException("Failed to get auth response from server response.");
        }

        await SaveTokens(authResponse);
        (_authStateProvider as CustomAuthenticationStateProvider)!.NotifyUserLogin(authResponse.AccessToken);

        return authResponse.AccessToken;
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", RefreshTokenKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", AccessTokenKey);
        (_authStateProvider as CustomAuthenticationStateProvider)!.NotifyUserLogout();
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var accessToken = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", AccessTokenKey);

        if (string.IsNullOrEmpty(accessToken) || IsTokenExpired(accessToken))
        {
            return await RefreshTokenAsync();
        }

        return accessToken;
    }

    private async Task SaveTokens(AuthResponseDTO authResponse)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", AccessTokenKey, authResponse.AccessToken);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", RefreshTokenKey, authResponse.RefreshToken);
    }

    private static bool IsTokenExpired(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        return jwtToken.ValidTo < DateTime.UtcNow;
    }

    private async Task<string> GetRefreshToken()
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", RefreshTokenKey);
    }
}
