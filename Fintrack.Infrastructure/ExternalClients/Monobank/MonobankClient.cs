using Fintrack.Domain.Entities;
using Fintrack.ExternalClients.Abstractions;
using System.Net;
using System.Net.Http.Json;

namespace Fintrack.Infrastructure.ExternalClients.Monobank;

public sealed class MonobankClient : IMonobankClient
{
    private readonly HttpClient _httpClient;

    public MonobankClient(IHttpClientFactory httpClientFactory)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);

        _httpClient = httpClientFactory.CreateClient("MonobankAPI");
    }

    public async Task<IEnumerable<FinancialOperation>> FetchFinancialOperationsAsync(string monobankAPIToken, DateTime from, DateTime to, Guid userId, Dictionary<int, Guid> mccToTransactionTypeId)
    {
        var fromUnix = ((DateTimeOffset)from).ToUnixTimeSeconds();
        var toUnix = ((DateTimeOffset)to).ToUnixTimeSeconds();

        // Monobank will respond with code 400 if the time frame is greater than 31 days + 1 hour (2,682,000 seconds)
        if (toUnix - fromUnix >= 2682000)
        {
            throw new ArgumentException("Time frame for transaction import shouldn't be longer than a month.");
        }

        var uri = $"/personal/statement/0/{fromUnix}/{toUnix}";
        _httpClient.DefaultRequestHeaders.Add("X-Token", monobankAPIToken);

        var financialOperations = new List<FinancialOperation>();
        var transactions = await GetTransactions(uri);

        if (transactions is not null)
        {
            financialOperations.AddRange(transactions.Select(e => e.ToFinancialOperation(mccToTransactionTypeId, userId)));
        }

        return financialOperations;
    }

    private async Task<IEnumerable<MonobankTransactionResponseDTO>?> GetTransactions(string uri)
    {
        var response = await _httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new HttpRequestException("Access forbidden. It looks like something is up with your Monobank API token.");
            }
            else
            {
                throw new HttpRequestException($"Response status code does not indicate success: {response.StatusCode}");
            }
        }

        return await response.Content.ReadFromJsonAsync<IEnumerable<MonobankTransactionResponseDTO>>();
    }
}
