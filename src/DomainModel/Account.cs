using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Account
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Adress { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
}