using Fintrack.Frontend.Services.Abstractions;
using Fintrack.Frontend.Utilities;
using System.Net.Http.Headers;

namespace Fintrack.Frontend.Services;

internal abstract class BaseService
{
    private protected readonly HttpClient _httpClient;
    private protected readonly IAuthService _authService;

    protected BaseService(IHttpClientFactory httpClientFactory, IAuthService authService)
    {
        _httpClient = httpClientFactory.CreateClient("FintrackAPI");
        _authService = authService;
    }

    private protected async Task AddAuthorizationHeaderAsync()
    {
        var token = await _authService.GetAccessTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    private protected async Task<HttpResponseMessage> SendAsync(string url, HttpMethod method)
    {
        return await SendAsync<object>(url, method, null!);
    }

    private protected async Task<HttpResponseMessage> SendAsync<T>(string url, HttpMethod method, T data)
    {
        await AddAuthorizationHeaderAsync();

        var response = await _httpClient.SendAsync(new HttpRequestMessage()
        {
            Method = method,
            RequestUri = new Uri(url, UriKind.Relative),
            Content = data == null ? null : JsonContent.Create(data, mediaType: null)
        });

        await response.CustomEnsureSuccessStatusCode();

        return response;
    }

    private protected async Task<T?> SendAsync<T>(string url, HttpMethod method)
    {
        var response = await SendAsync<T>(url, method, default!);

        return await response.Content.ReadFromJsonAsync<T>();
    }

    private protected async Task<T?> SendAsync<T, R>(string url, HttpMethod method, R data)
    {
        var response = await SendAsync(url, method, data);

        return await response.Content.ReadFromJsonAsync<T>();
    }
}
