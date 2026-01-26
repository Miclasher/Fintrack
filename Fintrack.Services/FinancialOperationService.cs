using Fintrack.Domain.Entities;
using Fintrack.Domain.Repositories;
using Fintrack.Services.Abstractions;
using Fintrack.Shared;
using Mapster;

namespace Fintrack.Services;

public sealed class FinancialOperationService : IFinancialOperationService
{
    private readonly IRepositoryManager _repositoryManager;

    public FinancialOperationService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager ?? throw new ArgumentNullException(nameof(repositoryManager));
    }

    public async Task<Guid> CreateAsync(Guid userId, FinancialOperationForCreateDTO finOp, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(finOp);

        var transactionTypes = await _repositoryManager.TransactionType.GetAllByUserAsync(userId, cancellationToken);
        if (!transactionTypes.Any())
        {
            throw new InvalidOperationException("No transaction types available for the user. Cannot create financial operation.");
        }

        if (finOp.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.");
        }

        var newFinOp = finOp.Adapt<FinancialOperation>();

        newFinOp.UserId = userId;

        await _repositoryManager.FinancialOperation.AddAsync(newFinOp, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        return newFinOp.Id;
    }

    public async Task DeleteAsync(Guid userId, Guid targetId, CancellationToken cancellationToken = default)
    {
        var target = await _repositoryManager.FinancialOperation.GetByIdAsync(targetId, cancellationToken)
            ?? throw new KeyNotFoundException($"Financial operation with id {targetId} was not found in database.");

        if (target.UserId != userId)
        {
            throw new AccessViolationException("Financial operation is owned by another user. Access denied.");
        }

        _repositoryManager.FinancialOperation.Remove(target, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<FinancialOperationDTO>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var finOps = await _repositoryManager.FinancialOperation.GetAllByUserAsync(userId, cancellationToken);

        return finOps
            .OrderByDescending(finop => finop.Date)
            .Adapt<IEnumerable<FinancialOperationDTO>>();
    }

    public async Task<FinancialOperationDTO> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var finOp = await _repositoryManager.FinancialOperation.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Financial operation with id {id} was not found in database.");

        if (finOp.UserId != userId)
        {
            throw new AccessViolationException("Financial operation is owned by another user. Access denied.");
        }

        return finOp.Adapt<FinancialOperationDTO>();
    }

    public async Task UpdateAsync(Guid userId, FinancialOperationDTO finOp, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(finOp);

        var finOpToUpdate = await _repositoryManager.FinancialOperation.GetByIdWithoutTransactionType(finOp.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Financial operation with id {finOp.Id} was not found in database.");

        if (finOpToUpdate.UserId != userId)
        {
            throw new AccessViolationException("Financial operation is owned by another user. Access denied.");
        }

        if (finOp.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.");
        }

        finOpToUpdate.Date = finOp.Date;
        finOpToUpdate.Amount = finOp.Amount;
        finOpToUpdate.TransactionTypeId = finOp.TransactionTypeId;
        finOpToUpdate.UserComment = finOp.UserComment;

        _repositoryManager.FinancialOperation.Update(finOpToUpdate, cancellationToken);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
