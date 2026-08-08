using AutoMapper;
using DddApiTemplate.Application.Common;
using DddApiTemplate.Application.DTOs;
using DddApiTemplate.Domain.Repositories;

namespace DddApiTemplate.Application.Queries.GetOrderById;

public sealed class GetOrderByIdQueryHandler(
    IOrderRepository orderRepository,
    IMapper mapper) : IQueryHandler<GetOrderByIdQuery, OrderDto?>
{
    public async Task<OrderDto?> HandleAsync(GetOrderByIdQuery query, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(query.OrderId, cancellationToken);
        return order is null ? null : mapper.Map<OrderDto>(order);
    }
}
