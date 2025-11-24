using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Domain
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;
}