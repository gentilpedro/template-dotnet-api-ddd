using DddApiTemplate.Domain.Common;
using DddApiTemplate.Domain.Exceptions;
using DddApiTemplate.Domain.ValueObjects;

namespace DddApiTemplate.Domain.Entities;

public sealed class OrderItem : Entity
{
    public Guid OrderId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; } = Money.Zero();

    public Money Total => UnitPrice.Multiply(Quantity);

    private OrderItem() { }

    internal OrderItem(string productName, int quantity, Money unitPrice)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("O nome do produto é obrigatório.");

        if (quantity <= 0)
            throw new DomainException("A quantidade deve ser maior que zero.");

        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}
