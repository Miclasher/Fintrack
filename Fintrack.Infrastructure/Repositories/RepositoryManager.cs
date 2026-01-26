using Fintrack.Domain.Repositories;

namespace Fintrack.Infrastructure.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly FintrackContext _context;
    private readonly Lazy<ITransactionTypeRepository> _lazyTransactionTypeRepository;
    private readonly Lazy<IFinancialOperationRepository> _lazyFinancialOperationRepository;
    private readonly Lazy<IUserRepository> _lazyUserRepository;
    private readonly Lazy<IJwtRefreshTokenRepository> _lazyRefreshTokenRepository;
    private readonly Lazy<ITransactionTypeTemplateRepository> _lazyTransactionTypeTemplateRepository;
    private readonly Lazy<IMccRepository> _lazyMccRepository;
    private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

    public RepositoryManager(FintrackContext context)
    {
        _context = context;
        _lazyTransactionTypeRepository = new Lazy<ITransactionTypeRepository>(() => new TransactionTypeRepository(context));
        _lazyFinancialOperationRepository = new Lazy<IFinancialOperationRepository>(() => new FinancialOperationRepository(context));
        _lazyUserRepository = new Lazy<IUserRepository>(() => new UserRepository(context));
        _lazyRefreshTokenRepository = new Lazy<IJwtRefreshTokenRepository>(() => new JwtRefreshTokenRepository(context));
        _lazyTransactionTypeTemplateRepository = new Lazy<ITransactionTypeTemplateRepository>(() => new TransactionTypeRepositoryTemplate(context));
        _lazyMccRepository = new Lazy<IMccRepository>(() => new MccRepository(context));
        _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(context));
    }

    public ITransactionTypeRepository TransactionType => _lazyTransactionTypeRepository.Value;

    public IFinancialOperationRepository FinancialOperation => _lazyFinancialOperationRepository.Value;

    public IUserRepository User => _lazyUserRepository.Value;

    public IJwtRefreshTokenRepository RefreshToken => _lazyRefreshTokenRepository.Value;
    public IMccRepository Mcc => _lazyMccRepository.Value;
    public ITransactionTypeTemplateRepository TransactionTypeTemplate => _lazyTransactionTypeTemplateRepository.Value;

    public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
}
