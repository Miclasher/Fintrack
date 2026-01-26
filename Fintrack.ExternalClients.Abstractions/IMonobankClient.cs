using Fintrack.Domain.Entities;

namespace Fintrack.ExternalClients.Abstractions;

public interface IMonobankClient
{
    Task<IEnumerable<FinancialOperation>> FetchFinancialOperationsAsync(string accountId, DateTime from, DateTime to, Guid userId, Dictionary<int, Guid> mccToTransactionTypeId);
}
