using DddApiTemplate.Domain.Common;

namespace DddApiTemplate.Domain.Events;

public sealed class OrderCreatedDomainEvent(Guid orderId, string customerName) : IDomainEvent
{
    public Guid OrderId { get; } = orderId;
    public string CustomerName { get; } = customerName;
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
