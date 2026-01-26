using Fintrack.Domain.Entities;
using Fintrack.Services;
using Fintrack.Shared;
using Moq;

namespace Fintrack.Tests;

[TestClass]
public class SummaryServiceTests : ServiceTestsBase
{
    private SummaryService _summaryService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _summaryService = new SummaryService(_mockRepositoryManager.Object);
    }

    [TestMethod]
    public async Task GetDateRangeSummaryAsyncTest1()
    {
        var userId = Guid.NewGuid();
        var dateRange = new DateRangeDTO { StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7)), EndDate = DateOnly.FromDateTime(DateTime.UtcNow) };
        var finOps = new List<FinancialOperation>
        {
            new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, Amount = 100, Date = DateTime.UtcNow, TransactionType = new TransactionType { IsExpense = false } },
            new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, Amount = 50, Date = DateTime.UtcNow, TransactionType = new TransactionType { IsExpense = true } }
        };

        _mockFinancialOperationRepository.Setup(r => r.GetAllByUserAndDateRangeAsync(userId, dateRange.StartDate, dateRange.EndDate, It.IsAny<CancellationToken>())).ReturnsAsync(finOps);

        var result = await _summaryService.GetDateRangeSummaryAsync(userId, dateRange);

        Assert.AreEqual(100, result.TotalIncome);
        Assert.AreEqual(50, result.TotalExpense);
        Assert.AreEqual(2, result.Operations.Count());
    }

    [TestMethod]
    public async Task GetDateRangeSummaryAsyncTest2()
    {
        var userId = Guid.NewGuid();
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _summaryService.GetDateRangeSummaryAsync(userId, null!));
    }

    [TestMethod]
    public async Task GetDaySummaryAsyncTest1()
    {
        var userId = Guid.NewGuid();
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var finOps = new List<FinancialOperation>
        {
            new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, Amount = 100, Date = DateTime.UtcNow, TransactionType = new TransactionType { IsExpense = false } },
            new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, Amount = 50, Date = DateTime.UtcNow, TransactionType = new TransactionType { IsExpense = true } }
        };

        _mockFinancialOperationRepository.Setup(r => r.GetAllByUserAndDateRangeAsync(userId, date, date, It.IsAny<CancellationToken>())).ReturnsAsync(finOps);

        var result = await _summaryService.GetDaySummaryAsync(userId, date);

        Assert.AreEqual(100, result.TotalIncome);
        Assert.AreEqual(50, result.TotalExpense);
        Assert.AreEqual(2, result.Operations.Count());
    }
}
