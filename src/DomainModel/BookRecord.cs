using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public enum BookRecordStatus
{
    ForReadingRoom,
    Borrowed,
    Available
}

public class BookRecord
{
    public int Id { get; set; }

    [Required]
    public BookEdition BookEdition { get; set; } = null!;

    public BookRecordStatus Status { get; set; }
}