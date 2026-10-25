using Alsiso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Products
{
    public sealed class GetProductByIdUseCase(IProductRepository productRepository)
    {
        public Task<Product?> ExecuteAsync(Guid id,
            CancellationToken cancellationToken)
        {
            return productRepository.GetByIdAsync(id, cancellationToken);

        }

    }
}
