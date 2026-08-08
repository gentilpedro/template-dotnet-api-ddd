using DddApiTemplate.Application.Commands.CreateOrder;
using DddApiTemplate.Application.Common;
using DddApiTemplate.Application.DTOs;
using DddApiTemplate.Application.Queries.GetOrderById;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DddApiTemplate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(
    ICommandHandler<CreateOrderCommand, OrderDto> createOrderHandler,
    IQueryHandler<GetOrderByIdQuery, OrderDto?> getOrderByIdHandler,
    IValidator<CreateOrderCommand> createOrderValidator) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<OrderDto>> Create(
        [FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await createOrderValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return ValidationProblem(new ValidationProblemDetails(
                validationResult.ToDictionary()));

        var order = await createOrderHandler.HandleAsync(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<OrderDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var order = await getOrderByIdHandler.HandleAsync(new GetOrderByIdQuery(id), cancellationToken);
        return order is null ? NotFound() : Ok(order);
    }
}
