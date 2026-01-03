using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class BorrowRecord : IEntity
{
    public int Id { get; init; }

    [Required]
    public DateTime BorrowDateTime { get; init; }

    [Required]
    public int BorrowerId { get; init; }

    public Client Borrower { get; private set; } = null!;

    [Required]
    public int LenderId { get; init; }

    public Employee Lender { get; private set; } = null!;

    public List<Book> BorrowedBooks { get; } = [];
}