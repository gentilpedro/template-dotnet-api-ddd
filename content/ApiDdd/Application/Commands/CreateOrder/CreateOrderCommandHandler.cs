using AutoMapper;
using DddApiTemplate.Application.Common;
using DddApiTemplate.Application.DTOs;
using DddApiTemplate.Application.Interfaces;
using DddApiTemplate.Domain.Entities;
using DddApiTemplate.Domain.Repositories;
using DddApiTemplate.Domain.ValueObjects;

namespace DddApiTemplate.Application.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : ICommandHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> HandleAsync(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        var order = Order.Create(command.CustomerName);

        foreach (var item in command.Items)
        {
            order.AddItem(item.ProductName, item.Quantity, Money.Create(item.UnitPrice));
        }

        await orderRepository.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<OrderDto>(order);
    }
}
