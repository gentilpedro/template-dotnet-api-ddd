namespace DddApiTemplate.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}
