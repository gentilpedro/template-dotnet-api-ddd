using DddApiTemplate.Domain.Common;
using DddApiTemplate.Domain.Events;
using DddApiTemplate.Domain.Exceptions;
using DddApiTemplate.Domain.ValueObjects;

namespace DddApiTemplate.Domain.Entities;

public enum OrderStatus
{
    Pending,
    Confirmed,
    Cancelled
}

public sealed class Order : AggregateRoot
{
    public string CustomerName { get; private set; } = string.Empty;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public Money Total => _items
        .Select(i => i.Total)
        .Aggregate(Money.Zero(), (acc, next) => acc.Add(next));

    private Order() { }

    private Order(string customerName)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new DomainException("O nome do cliente é obrigatório.");

        CustomerName = customerName;
        Status = OrderStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public static Order Create(string customerName)
    {
        var order = new Order(customerName);
        order.AddDomainEvent(new OrderCreatedDomainEvent(order.Id, order.CustomerName));
        return order;
    }

    public void AddItem(string productName, int quantity, Money unitPrice)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Só é possível adicionar itens a um pedido pendente.");

        _items.Add(new OrderItem(productName, quantity, unitPrice));
    }

    public void Confirm()
    {
        if (_items.Count == 0)
            throw new DomainException("Não é possível confirmar um pedido sem itens.");

        if (Status != OrderStatus.Pending)
            throw new DomainException("Somente pedidos pendentes podem ser confirmados.");

        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Cancelled)
            throw new DomainException("O pedido já está cancelado.");

        Status = OrderStatus.Cancelled;
    }
}
