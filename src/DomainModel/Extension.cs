using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Extension : IEntity
{
    public int Id { get; init; }

    [Required]
    public int DayCount { get; init; }

    public DateTime CreatedDateTime { get; init; }

    [Required]
    public int RequestedById { get; init; }

    public Client RequestedBy { get; private set; } = null!;

    public Extension(int requestedById, int dayCount)
    {
        RequestedById = requestedById;
        DayCount = dayCount;
        CreatedDateTime = DateTime.Now;
    }
}