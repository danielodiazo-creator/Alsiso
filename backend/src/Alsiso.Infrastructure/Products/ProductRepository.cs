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


        public async Task<Product?> GetByIdAsync
            (Guid id, CancellationToken cancellationToken)
        {
            return await dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(product => product.Id == id,
                cancellationToken);
        }

        public async Task UpdateAsync(
        Product product,
        CancellationToken cancellationToken)
        {
            dbContext.Products.Update(product);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
        {
            var product = await dbContext.Products
                .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

            if (product is null)
            {
                return false;
            }

            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

    }
}
