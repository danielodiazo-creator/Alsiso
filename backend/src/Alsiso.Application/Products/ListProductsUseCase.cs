using Alsiso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Products
{
    public sealed class ListProductsUseCase
    {
        private readonly IProductRepository _productRepository;

        public ListProductsUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<IReadOnlyList<Product>> ExecuteAsync(
            CancellationToken cancellationToken = default)
        {
            return _productRepository.GetAllAsync(cancellationToken);
        }
    }
}
