namespace DomainModel;

public interface IEntity<TId>
{
    TId Id { get; }
}