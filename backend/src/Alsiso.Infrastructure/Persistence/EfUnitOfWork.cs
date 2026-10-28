using Alsiso.Application.Abstractions;
using Alsiso.Infrastructure.Products.Persistence;

namespace Alsiso.Infrastructure.Persistence;

public sealed class EfUnitOfWork(
    AlsisoDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
