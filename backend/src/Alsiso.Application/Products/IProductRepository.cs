using Alsiso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Products
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetAllAsync
            (CancellationToken cancellationToken  = default);

        Task AddAsync(Product product, CancellationToken cancellationToken);

        Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

        Task UpdateAsync(Product product, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken);
    }
}
