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

        private readonly CreateProductUseCase _createProductUseCase;


        public ProductsController(
        ListProductsUseCase listProductsUseCase,
        CreateProductUseCase createProductUseCase)
        {
            _listProductsUseCase = listProductsUseCase;
            _createProductUseCase = createProductUseCase;
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

        [HttpPost]
        public async Task<ActionResult<ProductResponse>> Create(
        CreateProductRequest request,
        CancellationToken cancellationToken)
        {
            var command = new CreateProductCommand(
                request.Name,
                request.Description,
                request.Category,
                request.Price);

            var product = await _createProductUseCase.ExecuteAsync(
                command,
                cancellationToken);

            var response = new ProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Category,
                product.Price);

            return StatusCode(StatusCodes.Status201Created, response);
        }



    }
}
