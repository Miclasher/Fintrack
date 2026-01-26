using Fintrack.Domain.Entities;
using Fintrack.Services;
using Fintrack.Shared;
using Mapster;
using Moq;

namespace Fintrack.Tests;

[TestClass]
public class FinancialOperationServiceTests : ServiceTestsBase
{
    private FinancialOperationService _financialOperationService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _financialOperationService = new FinancialOperationService(_mockRepositoryManager.Object);
    }

    [TestMethod]
    public async Task CreateAsyncTest()
    {
        var userId = Guid.NewGuid();
        var finOpForCreate = new FinancialOperationForCreateDTO { Amount = 100, Date = DateTime.UtcNow, TransactionTypeId = Guid.NewGuid(), UserComment = "Test Comment" };
        var finOp = finOpForCreate.Adapt<FinancialOperation>();
        finOp.UserId = userId;

        _mockRepositoryManager.Setup(r => r.TransactionType.GetAllByUserAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<TransactionType> { new TransactionType { Id = finOp.TransactionTypeId, UserId = userId } });
        _mockRepositoryManager.Setup(r => r.FinancialOperation.AddAsync(It.IsAny<FinancialOperation>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _mockRepositoryManager.Setup(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _financialOperationService.CreateAsync(userId, finOpForCreate);

        _mockRepositoryManager.Verify(r => r.FinancialOperation.AddAsync(It.Is<FinancialOperation>(f => f.Amount == finOpForCreate.Amount && f.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
        _mockRepositoryManager.Verify(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsyncTest()
    {
        var userId = Guid.NewGuid();
        var finOpId = Guid.NewGuid();
        var finOp = new FinancialOperation { Id = finOpId, UserId = userId };

        _mockRepositoryManager.Setup(r => r.FinancialOperation.GetByIdAsync(finOpId, It.IsAny<CancellationToken>())).ReturnsAsync(finOp);
        _mockRepositoryManager.Setup(r => r.FinancialOperation.Remove(finOp, It.IsAny<CancellationToken>()));
        _mockRepositoryManager.Setup(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _financialOperationService.DeleteAsync(userId, finOpId);

        _mockRepositoryManager.Verify(r => r.FinancialOperation.Remove(It.Is<FinancialOperation>(f => f.Id == finOpId), It.IsAny<CancellationToken>()), Times.Once);
        _mockRepositoryManager.Verify(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task GetAllAsyncTest()
    {
        var userId = Guid.NewGuid();
        var finOps = new List<FinancialOperation>
        {
            new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, Amount = 100, Date = DateTime.UtcNow },
            new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, Amount = 200, Date = DateTime.UtcNow }
        };

        _mockRepositoryManager.Setup(r => r.FinancialOperation.GetAllByUserAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(finOps);

        var result = await _financialOperationService.GetAllAsync(userId);

        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.Any(f => f.Amount == 100));
        Assert.IsTrue(result.Any(f => f.Amount == 200));
    }

    [TestMethod]
    public async Task GetByIdAsyncTest()
    {
        var userId = Guid.NewGuid();
        var finOpId = Guid.NewGuid();
        var finOp = new FinancialOperation { Id = finOpId, UserId = userId, Amount = 100, Date = DateTime.UtcNow };

        _mockRepositoryManager.Setup(r => r.FinancialOperation.GetByIdAsync(finOpId, It.IsAny<CancellationToken>())).ReturnsAsync(finOp);

        var result = await _financialOperationService.GetByIdAsync(userId, finOpId);

        Assert.AreEqual(finOpId, result.Id);
        Assert.AreEqual(100, result.Amount);
    }

    [TestMethod]
    public async Task UpdateAsyncTest()
    {
        var userId = Guid.NewGuid();
        var finOpForUpdate = new FinancialOperationDTO { Id = Guid.NewGuid(), Amount = 200, Date = DateTime.UtcNow, TransactionTypeId = Guid.NewGuid(), UserComment = "Updated Comment" };
        var finOp = new FinancialOperation { Id = finOpForUpdate.Id, UserId = userId, Amount = 100, Date = DateTime.UtcNow, TransactionTypeId = finOpForUpdate.TransactionTypeId };

        _mockRepositoryManager.Setup(r => r.FinancialOperation.GetByIdAsync(finOpForUpdate.Id, It.IsAny<CancellationToken>())).ReturnsAsync(finOp);
        _mockRepositoryManager.Setup(r => r.FinancialOperation.Update(finOp, It.IsAny<CancellationToken>()));
        _mockRepositoryManager.Setup(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _financialOperationService.UpdateAsync(userId, finOpForUpdate);

        _mockRepositoryManager.Verify(r => r.FinancialOperation.Update(It.Is<FinancialOperation>(f => f.Amount == finOpForUpdate.Amount && f.UserComment == finOpForUpdate.UserComment), It.IsAny<CancellationToken>()), Times.Once);
        _mockRepositoryManager.Verify(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
