using DddApiTemplate.Domain.Common;
using DddApiTemplate.Domain.Exceptions;

namespace DddApiTemplate.Domain.ValueObjects;

public sealed class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency = "BRL")
    {
        if (amount < 0)
            throw new DomainException("O valor monetário não pode ser negativo.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("A moeda é obrigatória.");

        return new Money(amount, currency.ToUpperInvariant());
    }

    public static Money Zero(string currency = "BRL") => new(0, currency);

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Multiply(int factor) => new(Amount * factor, Currency);

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException("Não é possível operar valores em moedas diferentes.");
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount:0.00} {Currency}";
}
