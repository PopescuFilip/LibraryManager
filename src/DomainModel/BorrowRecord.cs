using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class BorrowRecord : IEntity
{
    public int Id { get; init; }

    [Required]
    public DateTime BorrowDateTime { get; init; }

    [Required]
    public DateTime BorrowedUntil { get; init; }

    [Required]
    public int BorrowerId { get; init; }

    public Client Borrower { get; private set; } = null!;

    [Required]
    public int LenderId { get; init; }

    public Employee Lender { get; private set; } = null!;

    [Required]
    public int BorrowedBookId { get; init; }

    public Book BorrowedBook { get; private set; }

    public BorrowRecord(int borrowerId, int lenderId, int borrowedBookId, DateTime borrowedUntil)
    {
        BorrowerId = borrowerId;
        LenderId = lenderId;
        BorrowedBookId = borrowedBookId;
        BorrowedUntil = borrowedUntil;
        BorrowDateTime = DateTime.Now;
    }
}