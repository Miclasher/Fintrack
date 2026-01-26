using Fintrack.Shared;

namespace Fintrack.Frontend.Utilities;

public static class CustomEnsureSuccesStatusCodeExtension
{
    public static async Task CustomEnsureSuccessStatusCode(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var error = await response.Content.ReadFromJsonAsync<ErrorDTO>();

        if (error is null || string.IsNullOrEmpty(error.Error))
        {
            throw new HttpRequestException("Unidentified server exception. Try again later");
        }

        throw new HttpRequestException(error.Error);
    }
}
