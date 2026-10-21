using Alsiso.Application.Products;
using Alsiso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Infrastructure.Products
{
    public sealed class InMemoryProductRepository : IProductRepository
    {
        private readonly IReadOnlyList<Product> _products = new List<Product>
        {
           new Product(
            Guid.Parse("11111111-1111-4111-8111-111111111111"),
            "Señal de salida",
            "Señal fotoluminiscente para rutas de evacuación.",
            "Señalización de emergencia",
            null)
        };

        public Task<IReadOnlyList<Product>> GetAllAsync(
        CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(_products);
        }

    }
}
