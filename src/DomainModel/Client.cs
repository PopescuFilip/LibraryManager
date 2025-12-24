using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Client : IEntity<int>
{
    public int Id { get; init; }

    [Required]
    public int AccountId { get; init; }

    public Account Account { get; private set; } = null!;

    public List<BorrowRecord> BorrowRecords { get; } = [];
}