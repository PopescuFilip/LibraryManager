using DomainModel;
using ServiceLayer.CRUD;
using ServiceLayer.UnitTests.TestHelpers;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class BorrowRecordQueryServiceTests
{
    private BorrowRecordQueryService _queryService = default!;
    private FakeRepository<BorrowRecord> _fakeRepository = default!;

    [TestInitialize]
    public void Init()
    {
        _fakeRepository = new FakeRepository<BorrowRecord>();
        _queryService = new BorrowRecordQueryService(_fakeRepository);
    }

    [TestMethod]
    public void GetBooksLendedTodayCount_ShouldReturnZero_WhenNoItemsPassFilter()
    {
        var employeeId = 3;
        var someOtherId = 33;
        var options = new List<(int LenderId, int BookCount, DateTime Date)>()
        {
            (someOtherId, 43, DateTime.Today),
            (someOtherId, 21, DateTime.Today),
            (employeeId, 20, DateTime.Today.AddDays(2)),
            (employeeId, 47, DateTime.Today.AddHours(-4)),
        };
        var borrowRecords = Generator.GenerateBorrowRecordsFrom(options);
        _fakeRepository.SetSourceValues(borrowRecords);

        var count = _queryService.GetBooksLendedTodayCount(employeeId);

        Assert.AreEqual(0, count);
    }

    [TestMethod]
    public void GetBooksLendedTodayCount_ShouldReturnCorrectCount()
    {
        var employeeId = 3;
        var count1 = 43;
        var count2 = 20;
        var count3 = 54;
        var total = count1 + count2 + count3;
        var someOtherId = 33;
        var options = new List<(int LenderId, int BookCount, DateTime Date)>()
        {
            (employeeId, count1, DateTime.Today),
            (someOtherId, 21, DateTime.Today),
            (employeeId, 60, DateTime.Today.AddDays(2)),
            (employeeId, count2, DateTime.Today.AddHours(4)),
            (employeeId, 47, DateTime.Today.AddHours(-4)),
            (employeeId, count3, DateTime.Today.AddHours(20)),
        };
        var borrowRecords = Generator.GenerateBorrowRecordsFrom(options);
        _fakeRepository.SetSourceValues(borrowRecords);

        var count = _queryService.GetBooksLendedTodayCount(employeeId);

        Assert.AreEqual(total, count);
    }
}