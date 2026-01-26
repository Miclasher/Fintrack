namespace Fintrack.Domain.Repositories;

public interface IRepositoryManager
{
    ITransactionTypeRepository TransactionType { get; }
    IFinancialOperationRepository FinancialOperation { get; }
    IUserRepository User { get; }
    IJwtRefreshTokenRepository RefreshToken { get; }
    IMccRepository Mcc { get; }
    ITransactionTypeTemplateRepository TransactionTypeTemplate { get; }
    IUnitOfWork UnitOfWork { get; }
}
