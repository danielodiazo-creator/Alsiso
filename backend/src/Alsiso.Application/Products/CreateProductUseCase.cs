using Alsiso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Products
{
    public sealed class CreateProductUseCase(IProductRepository productRepository)
    {
        public async Task<Product> ExecuteAsync(
            CreateProductCommand command,
            CancellationToken cancellationToken)
        {
            var product = new Product(
                Guid.NewGuid(),
                command.Name,
                command.Description,
                command.Category,
                command.Price);
            
            await productRepository.AddAsync(product, cancellationToken);

            return product;
        }
    }
}