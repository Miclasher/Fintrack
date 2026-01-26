using Fintrack.Domain.Repositories;
using Moq;

namespace Fintrack.Tests;

public class ServiceTestsBase
{
    private protected Mock<IRepositoryManager> _mockRepositoryManager;
    private protected Mock<IJwtRefreshTokenRepository> _mockJwtRefreshTokenRepository;
    private protected Mock<ITransactionTypeRepository> _mockTransactionTypeRepository;
    private protected Mock<IFinancialOperationRepository> _mockFinancialOperationRepository;
    private protected Mock<IUserRepository> _mockUserRepository;
    private protected Mock<IUnitOfWork> _mockUnitOfWork;

    public ServiceTestsBase()
    {
        _mockRepositoryManager = new Mock<IRepositoryManager>();
        _mockJwtRefreshTokenRepository = new Mock<IJwtRefreshTokenRepository>();
        _mockTransactionTypeRepository = new Mock<ITransactionTypeRepository>();
        _mockFinancialOperationRepository = new Mock<IFinancialOperationRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockRepositoryManager.Setup(e => e.RefreshToken).Returns(_mockJwtRefreshTokenRepository.Object);
        _mockRepositoryManager.Setup(e => e.TransactionType).Returns(_mockTransactionTypeRepository.Object);
        _mockRepositoryManager.Setup(e => e.FinancialOperation).Returns(_mockFinancialOperationRepository.Object);
        _mockRepositoryManager.Setup(e => e.User).Returns(_mockUserRepository.Object);
        _mockRepositoryManager.Setup(e => e.UnitOfWork).Returns(_mockUnitOfWork.Object);
    }
}
