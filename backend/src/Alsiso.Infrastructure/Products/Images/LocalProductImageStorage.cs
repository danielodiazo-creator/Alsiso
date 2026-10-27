using Alsiso.Application.Products;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Alsiso.Infrastructure.Products.Images
{
    public sealed class LocalProductImageStorage(string imageDirectory)
        : IProductImageStorage
    {
        public async Task<string> SaveAsync(
            Stream imageStream,
            string contentType,
            CancellationToken cancellationToken)
        {
            if (!string.Equals(
                    contentType,
                    "image/png",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Solo se permiten imágenes PNG.",
                    nameof(contentType));
            }

            Directory.CreateDirectory(imageDirectory);

            var fileName = $"{Guid.NewGuid():N}.png";
            var filePath = Path.Combine(imageDirectory, fileName);

            try
            {
                await using var outputStream = new FileStream(
                    filePath,
                    FileMode.CreateNew,
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize: 81920,
                    useAsync: true);

                await imageStream.CopyToAsync(
                    outputStream,
                    cancellationToken);
            }
            catch
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                throw;
            }

            return $"/uploads/products/{fileName}";
        }

        public Task DeleteAsync(
            string imageReference,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var fileName = Path.GetFileName(imageReference);
            var filePath = Path.Combine(imageDirectory, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }
    }
}