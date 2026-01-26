using Fintrack.Frontend.Services.Abstractions;
using Fintrack.Shared;

namespace Fintrack.Frontend.Services;

internal sealed class FinancialOperationService : BaseService, IFinancialOperationService
{
    public FinancialOperationService(IHttpClientFactory httpClientFactory, IAuthService authService) : base(httpClientFactory, authService)
    {
    }

    public async Task CreateAsync(FinancialOperationForCreateDTO financialOperation)
    {
        ArgumentNullException.ThrowIfNull(financialOperation);

        await SendAsync("api/FinancialOperation", HttpMethod.Post, financialOperation);
    }

    public async Task DeleteAsync(Guid id)
    {
        await SendAsync($"api/FinancialOperation/{id}", HttpMethod.Delete);
    }

    public async Task<IEnumerable<FinancialOperationDTO>> GetAllAsync()
    {
        var financialOperations = await SendAsync<List<FinancialOperationDTO>>("api/FinancialOperation", HttpMethod.Get);

        return financialOperations ?? throw new InvalidDataException("Failed to get financial operations from server response.");
    }

    public async Task<FinancialOperationDTO> GetByIdAsync(Guid id)
    {
        var financialOperation = await SendAsync<FinancialOperationDTO>($"api/FinancialOperation/{id}", HttpMethod.Get);

        return financialOperation ?? throw new InvalidDataException("Failed to get financial operation from server response.");
    }

    public async Task UpdateAsync(FinancialOperationDTO financialOperation)
    {
        await SendAsync("api/FinancialOperation", HttpMethod.Put, financialOperation);
    }
}
