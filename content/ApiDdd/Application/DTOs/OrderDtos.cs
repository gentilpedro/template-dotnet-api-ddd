namespace DddApiTemplate.Application.DTOs;

public sealed record OrderItemDto(string ProductName, int Quantity, decimal UnitPrice, decimal Total);

public sealed record OrderDto(
    Guid Id,
    string CustomerName,
    string Status,
    decimal Total,
    DateTime CreatedAtUtc,
    IReadOnlyCollection<OrderItemDto> Items);
