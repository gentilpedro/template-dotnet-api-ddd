using DddApiTemplate.Domain.Entities;
using DddApiTemplate.Domain.Repositories;
using DddApiTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DddApiTemplate.Infrastructure.Repositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await context.Orders.AddAsync(order, cancellationToken);
    }
}
