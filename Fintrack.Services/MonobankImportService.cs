using Fintrack.Domain.Repositories;
using Fintrack.ExternalClients.Abstractions;
using Fintrack.Services.Abstractions;
using Fintrack.Shared;

namespace Fintrack.Services;

public sealed class MonobankImportService : IMonobankImportService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMonobankClient _monobankClient;

    public MonobankImportService(IRepositoryManager repositoryManager, IMonobankClient monobankClient)
    {
        _repositoryManager = repositoryManager ?? throw new ArgumentNullException(nameof(repositoryManager));
        _monobankClient = monobankClient ?? throw new ArgumentNullException(nameof(monobankClient));
    }

    public async Task<SummaryDTO> ImportFinancialOperations(Guid userId, DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        var user = await _repositoryManager.User.GetByIdAsync(userId, cancellationToken)
            ?? throw new InvalidDataException("Cannot find user with provided id.");

        if (string.IsNullOrEmpty(user.MonobankAccountId))
        {
            throw new InvalidDataException("User does not have monobank account linked.");
        }

        var mccToTransactionTypeIdDict = await _repositoryManager.TransactionType.GetMccToTransactionTypeIdDictAsync(userId, cancellationToken);

        if (mccToTransactionTypeIdDict is null || mccToTransactionTypeIdDict.Count == 0)
        {
            throw new InvalidDataException("Cannot find TransactionTypes associated with MCCs.");
        }

        var finOps
            = await _monobankClient.FetchFinancialOperationsAsync(user.MonobankAccountId, from, to, userId, mccToTransactionTypeIdDict)
            ?? throw new InvalidDataException("Financial operations cannot be null.");

        var guids = await _repositoryManager.FinancialOperation.GetAllIdsByUserAndDateRangeAsync(userId,
            DateOnly.FromDateTime(from),
            DateOnly.FromDateTime(to),
            cancellationToken);

        await SaveFinancialOperationsAsync(finOps, guids, cancellationToken);

        finOps = await IncludeTransactionTypeObjectsAsync(userId, finOps, cancellationToken);

        return SummaryService.GenerateSummary(finOps);
    }

    private async Task SaveFinancialOperationsAsync(IEnumerable<Domain.Entities.FinancialOperation> finOps, HashSet<Guid> guids, CancellationToken cancellationToken)
    {
        finOps = finOps.Where(e => !guids.Contains(e.Id));

        await _repositoryManager.FinancialOperation.AddRangeAsync(finOps, cancellationToken);
        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<IEnumerable<Domain.Entities.FinancialOperation>> IncludeTransactionTypeObjectsAsync(Guid userId, IEnumerable<Domain.Entities.FinancialOperation> finOps, CancellationToken cancellationToken)
    {
        var transactionTypeIds = finOps.Select(e => e.TransactionTypeId).Distinct().ToList();
        var transactionTypesDict = await _repositoryManager.TransactionType.GetDictByIdListAsync(userId, transactionTypeIds, cancellationToken);

        return finOps.Select(selector: e =>
        {
            e.TransactionType = transactionTypesDict[e.TransactionTypeId];
            return e;
        });
    }
}
