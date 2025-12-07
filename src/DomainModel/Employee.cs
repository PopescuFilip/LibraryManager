using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Employee
{
    public int Id { get; set; }

    [Required]
    public Account Account { get; set; } = null!;

    public List<BorrowRecord> BorrowRecords { get; set; } = [];
}