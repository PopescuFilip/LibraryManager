using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Account : IEntity
{
    public int Id { get; init; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Address { get; set; } = null!;

    [StringLength(50)]
    public string? Email { get; set; }

    [StringLength(12)]
    public string? PhoneNumber { get; set; }

    public Account(string name, string address, string? email, string? phoneNumber) =>
        (Name, Address, Email, PhoneNumber) = (name, address, email, phoneNumber);
}