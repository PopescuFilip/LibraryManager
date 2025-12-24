using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Employee : IEntity<int>
{
    public int Id { get; init; }

    [Required]
    public int AccountId { get; init; }

    public Account Account { get; set; } = null!;

    public List<BorrowRecord> BorrowRecords { get; } = [];
}