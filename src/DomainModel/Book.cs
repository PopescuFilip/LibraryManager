using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public enum BookStatus
{
    ForReadingRoom,
    Borrowed,
    Available
}

public class Book : IEntity
{
    public int Id { get; init; }

    [Required]
    public BookStatus Status { get; set; }

    [Required]
    public int BookEditionId { get; init; }

    public BookEdition BookEdition { get; private set; } = null!;

    public int? BorrowedById { get; set; }

    public Client? BorrowedBy { get; set; }

    public Book(BookStatus status, int bookEditionId) =>
        (Status, BookEditionId) = (status, bookEditionId);
}