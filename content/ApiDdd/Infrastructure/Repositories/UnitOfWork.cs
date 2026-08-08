using DddApiTemplate.Application.Interfaces;
using DddApiTemplate.Infrastructure.Data;

namespace DddApiTemplate.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}
