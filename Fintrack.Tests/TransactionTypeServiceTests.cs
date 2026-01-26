using Fintrack.Domain.Entities;
using Fintrack.Services;
using Fintrack.Shared;
using Mapster;
using Moq;

namespace Fintrack.Tests;

[TestClass]
public class TransactionTypeServiceTests : ServiceTestsBase
{
    private TransactionTypeService _transactionTypeService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _transactionTypeService = new TransactionTypeService(_mockRepositoryManager.Object);
    }

    [TestMethod]
    public async Task CreateAsyncTest()
    {
        var userId = Guid.NewGuid();
        var transTypeForCreate = new TransactionTypeForCreateDTO { Name = "Test Type", IsExpense = true };
        var transType = transTypeForCreate.Adapt<TransactionType>();
        transType.UserId = userId;

        _mockRepositoryManager.Setup(r => r.TransactionType.AddAsync(It.IsAny<TransactionType>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _mockRepositoryManager.Setup(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _transactionTypeService.CreateAsync(userId, transTypeForCreate);

        _mockRepositoryManager.Verify(r => r.TransactionType.AddAsync(It.Is<TransactionType>(t => t.Name == transTypeForCreate.Name && t.IsExpense == transTypeForCreate.IsExpense && t.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
        _mockRepositoryManager.Verify(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsyncTest()
    {
        var userId = Guid.NewGuid();
        var transTypeId = Guid.NewGuid();
        var transType = new TransactionType { Id = transTypeId, UserId = userId, FinancialOperations = new List<FinancialOperation>() };

        _mockRepositoryManager.Setup(r => r.TransactionType.GetByIdAsync(transTypeId, It.IsAny<CancellationToken>())).ReturnsAsync(transType);
        _mockRepositoryManager.Setup(r => r.TransactionType.Remove(transType, It.IsAny<CancellationToken>()));
        _mockRepositoryManager.Setup(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _transactionTypeService.DeleteAsync(userId, transTypeId);

        _mockRepositoryManager.Verify(r => r.TransactionType.Remove(It.Is<TransactionType>(t => t.Id == transTypeId), It.IsAny<CancellationToken>()), Times.Once);
        _mockRepositoryManager.Verify(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task GetAllAsyncTest()
    {
        var userId = Guid.NewGuid();
        var transTypes = new List<TransactionType>
        {
            new TransactionType { Id = Guid.NewGuid(), UserId = userId, Name = "Type 1", IsExpense = false },
            new TransactionType { Id = Guid.NewGuid(), UserId = userId, Name = "Type 2", IsExpense = true }
        };

        _mockRepositoryManager.Setup(r => r.TransactionType.GetAllByUserAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(transTypes);

        var result = await _transactionTypeService.GetAllAsync(userId);

        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.Any(t => t.Name == "Type 1"));
        Assert.IsTrue(result.Any(t => t.Name == "Type 2"));
    }

    [TestMethod]
    public async Task GetByIdAsyncTest()
    {
        var userId = Guid.NewGuid();
        var transTypeId = Guid.NewGuid();
        var transType = new TransactionType { Id = transTypeId, UserId = userId, Name = "Test Type", IsExpense = true };

        _mockRepositoryManager.Setup(r => r.TransactionType.GetByIdAsync(transTypeId, It.IsAny<CancellationToken>())).ReturnsAsync(transType);

        var result = await _transactionTypeService.GetByIdAsync(userId, transTypeId);

        Assert.AreEqual(transTypeId, result.Id);
        Assert.AreEqual("Test Type", result.Name);
    }

    [TestMethod]
    public async Task UpdateAsyncTest()
    {
        var userId = Guid.NewGuid();
        var transTypeForUpdate = new TransactionTypeDTO { Id = Guid.NewGuid(), Name = "Updated Type", IsExpense = false };
        var transType = new TransactionType { Id = transTypeForUpdate.Id, UserId = userId, Name = "Old Type", IsExpense = true };

        _mockRepositoryManager.Setup(r => r.TransactionType.GetByIdAsync(transTypeForUpdate.Id, It.IsAny<CancellationToken>())).ReturnsAsync(transType);
        _mockRepositoryManager.Setup(r => r.TransactionType.Update(transType, It.IsAny<CancellationToken>()));
        _mockRepositoryManager.Setup(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _transactionTypeService.UpdateAsync(userId, transTypeForUpdate);

        _mockRepositoryManager.Verify(r => r.TransactionType.Update(It.Is<TransactionType>(t => t.Name == transTypeForUpdate.Name && t.IsExpense == transTypeForUpdate.IsExpense), It.IsAny<CancellationToken>()), Times.Once);
        _mockRepositoryManager.Verify(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
