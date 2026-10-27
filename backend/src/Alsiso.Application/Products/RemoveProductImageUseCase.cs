using Alsiso.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Alsiso.Application.Products
{
    public sealed class RemoveProductImageUseCase(
        IProductRepository productRepository,
        IProductImageStorage productImageStorage)
    {
        public async Task<Product?> ExecuteAsync(
            Guid productId,
            CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(
                productId,
                cancellationToken);

            if (product is null)
            {
                return null;
            }

            var previousImageReference = product.ImageUrl;

            if (previousImageReference is null)
            {
                return product;
            }

            product.SetImageUrl(null);

            await productRepository.UpdateAsync(
                product,
                cancellationToken);

            await productImageStorage.DeleteAsync(
                previousImageReference,
                cancellationToken);

            return product;
        }
    }
}