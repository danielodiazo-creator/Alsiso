using Alsiso.Application.Products;
using Alsiso.Domain.Entities;
using Alsiso.Infrastructure.Products.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Infrastructure.Products
{
    public sealed class ProductRepository(AlsisoDbContext dbContext) : IProductRepository
    {
        public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await dbContext.Products.AsNoTracking()
                  .ToListAsync(cancellationToken);

        }


        public async Task AddAsync(
        Product product,
        CancellationToken cancellationToken)
        {
            await dbContext.Products.AddAsync(product, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
