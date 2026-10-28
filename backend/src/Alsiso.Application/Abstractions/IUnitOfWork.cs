using System.Threading;
using System.Threading.Tasks;

namespace Alsiso.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}