using Alsiso.Domain.Entities;

namespace Alsiso.Application.Products
{
    public sealed class UploadProductImageUseCase(
        IProductRepository productRepository,
        IProductImageStorage productImageStorage)
    {
        public async Task<Product?> ExecuteAsync(
            UploadProductImageCommand command,
            CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(
                command.ProductId,
                cancellationToken);

            if (product is null)
            {
                return null;
            }

            var previousImageUrl = product.ImageUrl;

            var newImageUrl = await productImageStorage.SaveAsync(
                command.ImageStream,
                command.ContentType,
                cancellationToken);

            product.SetImageUrl(newImageUrl);

            await productRepository.UpdateAsync(
                product,
                cancellationToken);

            if (previousImageUrl is not null)
            {
                await productImageStorage.DeleteAsync(
                    previousImageUrl,
                    cancellationToken);
            }

            return product;
        }
    }
}