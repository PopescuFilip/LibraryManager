using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Extension : IEntity
{
    public int Id { get; init; }

    [Required]
    public int DayCount { get; init; }

    public DateTime CreatedDateTime { get; init; }

    public Extension(int dayCount, DateTime createdDateTime) =>
        (DayCount, CreatedDateTime) = (dayCount, createdDateTime);
}