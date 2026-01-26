using Fintrack.Frontend.Services.Abstractions;
using Fintrack.Shared;

namespace Fintrack.Frontend.Services;

internal sealed class TransactionTypeService : BaseService, ITransactionTypeService
{
    public TransactionTypeService(IHttpClientFactory httpClientFactory, IAuthService authService) : base(httpClientFactory, authService)
    {
    }

    public async Task CreateAsync(TransactionTypeForCreateDTO transactionType)
    {
        ArgumentNullException.ThrowIfNull(transactionType);

        await SendAsync("api/TransactionType", HttpMethod.Post, transactionType);
    }

    public async Task DeleteAsync(Guid id)
    {
        await SendAsync($"api/TransactionType/{id}", HttpMethod.Delete);
    }

    public async Task<IEnumerable<TransactionTypeDTO>> GetAllAsync()
    {
        var transactionTypes = await SendAsync<List<TransactionTypeDTO>>("api/TransactionType", HttpMethod.Get);

        return transactionTypes ?? throw new InvalidDataException("Failed to get transaction types from server response.");
    }

    public async Task<TransactionTypeDTO> GetByIdAsync(Guid id)
    {
        var transactionType = await SendAsync<TransactionTypeDTO>($"api/TransactionType/{id}", HttpMethod.Get);

        return transactionType ?? throw new InvalidDataException("Failed to get transaction type from server response.");
    }

    public async Task UpdateAsync(TransactionTypeDTO transactionType)
    {
        ArgumentNullException.ThrowIfNull(transactionType);

        await SendAsync("api/TransactionType", HttpMethod.Put, transactionType);
    }
}
