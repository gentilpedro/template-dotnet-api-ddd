namespace DddApiTemplate.Application.Commands.CreateOrder;

public sealed record CreateOrderItemDto(string ProductName, int Quantity, decimal UnitPrice);

public sealed record CreateOrderCommand(string CustomerName, List<CreateOrderItemDto> Items);
