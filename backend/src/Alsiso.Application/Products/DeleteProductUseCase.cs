using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Products
{
    public sealed class DeleteProductUseCase(IProductRepository productRepository)
    {
        public Task<bool> ExecuteAsync(
        Guid id,
        CancellationToken cancellationToken)
        {
            return productRepository.DeleteAsync(id, cancellationToken);
        }

    }
}
