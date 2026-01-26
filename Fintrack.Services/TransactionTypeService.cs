using Fintrack.Domain.Entities;
using Fintrack.Domain.Repositories;
using Fintrack.Services.Abstractions;
using Fintrack.Shared;
using Mapster;

namespace Fintrack.Services;

public sealed class TransactionTypeService : ITransactionTypeService
{
    private readonly IRepositoryManager _repositoryManager;

    public TransactionTypeService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager ?? throw new ArgumentNullException(nameof(repositoryManager));
    }

    public async Task<Guid> CreateAsync(Guid userId, TransactionTypeForCreateDTO transType, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transType);

        var newTransType = transType.Adapt<TransactionType>();

        newTransType.UserId = userId;

        await _repositoryManager.TransactionType.AddAsync(newTransType, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        return newTransType.Id;
    }

    public async Task DeleteAsync(Guid userId, Guid targetId, CancellationToken cancellationToken = default)
    {
        var target = await _repositoryManager.TransactionType.GetByIdAsync(targetId, cancellationToken)
            ?? throw new KeyNotFoundException($"Transaction type with id {targetId} was not found in database.");

        if (target.UserId != userId)
        {
            throw new AccessViolationException("Transaction type is owned by another user. Access denied.");
        }
        if (target.FinancialOperations.Any())
        {
            throw new InvalidOperationException($"Cannot delete transaction type with id {targetId} because it has associated financial operations.");
        }
        if (target.Mccs.Any())
        {
            throw new InvalidOperationException($"Cannot delete transaction type with id {targetId} because it was automatically generated and has MCCs linked to it.");
        }

        _repositoryManager.TransactionType.Remove(target, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<TransactionTypeDTO>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var transTypes = await _repositoryManager.TransactionType.GetAllByUserAsync(userId, cancellationToken);

        return transTypes.Adapt<IEnumerable<TransactionTypeDTO>>();
    }

    public async Task<TransactionTypeDTO> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var transType = await _repositoryManager.TransactionType.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Transaction type with id {id} was not found in database.");

        if (transType.UserId != userId)
        {
            throw new AccessViolationException("Transaction type is owned by another user. Access denied.");
        }

        return transType.Adapt<TransactionTypeDTO>();
    }

    public async Task UpdateAsync(Guid userId, TransactionTypeDTO transType, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transType);
        var transTypeToUpdate = await _repositoryManager.TransactionType.GetByIdAsync(transType.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Transaction type with id {transType.Id} was not found in database.");

        if (transTypeToUpdate.UserId != userId)
        {
            throw new AccessViolationException("Transaction type is owned by another user. Access denied.");
        }

        transTypeToUpdate.Name = transType.Name;
        transTypeToUpdate.IsExpense = transType.IsExpense;

        _repositoryManager.TransactionType.Update(transTypeToUpdate, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
