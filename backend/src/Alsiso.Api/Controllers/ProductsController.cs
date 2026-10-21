using Microsoft.AspNetCore.Mvc;
using Alsiso.Application.Products;
using Alsiso.Api.Dtos;

namespace Alsiso.Api.Controllers
{


    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/products")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly ListProductsUseCase _listProductsUseCase;

        public ProductsController(ListProductsUseCase listProductsUseCase)
        {
            _listProductsUseCase = listProductsUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetAll(
            CancellationToken cancellationToken)
        {
            var products = await _listProductsUseCase.ExecuteAsync(cancellationToken);

            var response = products
                .Select(product => new ProductResponse(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Category,
                    product.Price))
                .ToList();

            return Ok(response);
        }

    }
}
