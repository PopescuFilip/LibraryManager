using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class BorrowRecord : IEntity<int>
{
    public int Id { get; set; }

    public DateTime BorrowDateTime { get; set; }

    [Required]
    public Client Borrower { get; set; } = null!;

    [Required]
    public Employee Lender { get; set; } = null!;

    public List<BookRecord> BorrowedBookRecords { get; set; } = [];
}