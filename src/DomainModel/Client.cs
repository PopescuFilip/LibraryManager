using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Client
{
    public int Id { get; set; }

    [Required]
    public Account Account { get; set; } = null!;

    public List<BorrowRecord> BorrowRecords { get; set; } = [];
}