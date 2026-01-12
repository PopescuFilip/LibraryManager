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
    public void GetActiveBorrowRecordWithBook_ShouldReturnNull_WhenRecordIsNotFound()
    {
        var clientId = 43;
        var bookId = 143;
        var borrowRecords = Enumerable.Range(0, 10)
            .Select(id => new BorrowRecord(id, id, id, DateTime.Now.AddDays(1)))
            .ToList();
        _fakeRepository.SetSourceValues(borrowRecords);

        var borrowRecord = _queryService.GetActiveBorrowRecordWithBook(clientId, bookId);

        Assert.IsNull(borrowRecord);
    }

    [TestMethod]
    public void GetActiveBorrowRecordWithBook_ShouldReturnBorrowRecord_WhenItExists()
    {
        var clientId = 43;
        var bookId = 143;
        var interestingRecord = new BorrowRecord(clientId, 1, bookId, DateTime.Now.AddDays(1));
        var borrowRecords = Enumerable.Range(0, 10)
            .Select(id => new BorrowRecord(id, id, id, DateTime.Now.AddDays(1)))
            .Concat([interestingRecord])
            .ToList();
        _fakeRepository.SetSourceValues(borrowRecords);

        var borrowRecord = _queryService.GetActiveBorrowRecordWithBook(clientId, bookId);

        Assert.IsNotNull(borrowRecord);
        Assert.AreEqual(interestingRecord, borrowRecord);
    }

    [TestMethod]
    public void GetBooksBorrowedInPeriodCount_ShouldReturnCorrectCountForSpecifiedEditionId()
    {
        var clientId = 3;
        var count1 = 43;
        var count2 = 20;
        var count3 = 54;
        var total = count1 + count2 + count3;
        var someOtherId = 33;
        var start = DateTime.Today;
        var end = DateTime.Today.AddDays(1);
        var editionId = 555;
        var otherEditionId = 788;
        var options = new List<(int BorrowerId, int EditionId, int BookCount, DateTime Date)>()
        {
            (clientId, editionId, count1, DateTime.Today),
            (someOtherId, otherEditionId, 21, DateTime.Today),
            (clientId, otherEditionId, 60, DateTime.Today.AddDays(2)),
            (clientId, editionId, count2, DateTime.Today.AddHours(4)),
            (clientId, otherEditionId, 47, DateTime.Today.AddHours(-4)),
            (clientId, editionId, count3, DateTime.Today.AddHours(20)),
        };
        var borrowRecords = Generator.GenerateBorrowRecordsForBorrower(options);
        _fakeRepository.SetSourceValues(borrowRecords);

        var count = _queryService.GetBooksBorrowedInPeriodCount(clientId, editionId, start, end);

        Assert.AreEqual(total, count);
    }

    [TestMethod]
    public void GetBooksBorrowedInPeriodCount_ShouldReturnCorrectCount()
    {
        var clientId = 3;
        var count1 = 43;
        var count2 = 20;
        var count3 = 54;
        var total = count1 + count2 + count3;
        var someOtherId = 33;
        var start = DateTime.Today;
        var end = DateTime.Today.AddDays(1);
        var options = new List<(int BorrowerId, int BookCount, DateTime Date)>()
        {
            (clientId, count1, DateTime.Today),
            (someOtherId, 21, DateTime.Today),
            (clientId, 60, DateTime.Today.AddDays(2)),
            (clientId, count2, DateTime.Today.AddHours(4)),
            (clientId, 47, DateTime.Today.AddHours(-4)),
            (clientId, count3, DateTime.Today.AddHours(20)),
        };
        var borrowRecords = Generator.GenerateBorrowRecordsForBorrower(options);
        _fakeRepository.SetSourceValues(borrowRecords);

        var count = _queryService.GetBooksBorrowedInPeriodCount(clientId, start, end);

        Assert.AreEqual(total, count);
    }

    [TestMethod]
    public void GetBooksBorrowedTodayCount_ShouldReturnCorrectCount()
    {
        var clientId = 3;
        var count1 = 43;
        var count2 = 20;
        var count3 = 54;
        var total = count1 + count2 + count3;
        var someOtherId = 33;
        var options = new List<(int BorrowerId, int BookCount, DateTime Date)>()
        {
            (clientId, count1, DateTime.Today),
            (someOtherId, 21, DateTime.Today),
            (clientId, 60, DateTime.Today.AddDays(2)),
            (clientId, count2, DateTime.Today.AddHours(4)),
            (clientId, 47, DateTime.Today.AddHours(-4)),
            (clientId, count3, DateTime.Today.AddHours(20)),
        };
        var borrowRecords = Generator.GenerateBorrowRecordsForBorrower(options);
        _fakeRepository.SetSourceValues(borrowRecords);

        var count = _queryService.GetBooksBorrowedTodayCount(clientId);

        Assert.AreEqual(total, count);
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
        var borrowRecords = Generator.GenerateBorrowRecordsForLender(options);
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
        var borrowRecords = Generator.GenerateBorrowRecordsForLender(options);
        _fakeRepository.SetSourceValues(borrowRecords);

        var count = _queryService.GetBooksLendedTodayCount(employeeId);

        Assert.AreEqual(total, count);
    }
}