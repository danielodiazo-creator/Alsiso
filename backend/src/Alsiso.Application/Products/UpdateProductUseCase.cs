using Alsiso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Products
{
    public sealed class UpdateProductUseCase(
        IProductRepository productRepository)
    {

        public async Task<Product?> ExecuteAsync(
        UpdateProductCommand command,
        CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(
                command.Id,
                cancellationToken);

            if (product is null)
            {
                return null;
            }

            product.UpdateDetails(
                command.Name,
                command.Description,
                command.Category,
                command.Price);

            await productRepository.UpdateAsync(
                product,
                cancellationToken);

            return product;
        }



    }
}
