using Microsoft.AspNetCore.Mvc;
using Alsiso.Application.Products;
using Alsiso.Api.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Alsiso.Api.Controllers
{


    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/products")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly ListProductsUseCase _listProductsUseCase;

        private readonly CreateProductUseCase _createProductUseCase;

        private readonly GetProductByIdUseCase _getProductByIdUseCase;

        private readonly UpdateProductUseCase _updateProductUseCase;

        private readonly DeleteProductUseCase _deleteProductUseCase;

        public ProductsController(
        ListProductsUseCase listProductsUseCase,
        CreateProductUseCase createProductUseCase,
        GetProductByIdUseCase getProductByIdUseCase,
        UpdateProductUseCase updateProductUseCase,
        DeleteProductUseCase deleteProductUseCase )
        {
            _listProductsUseCase = listProductsUseCase;
            _createProductUseCase = createProductUseCase;
            _getProductByIdUseCase = getProductByIdUseCase;
            _updateProductUseCase = updateProductUseCase;
            _deleteProductUseCase = deleteProductUseCase;
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

        [Authorize(Roles = "Admin")]
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

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
        {
            var product = await _getProductByIdUseCase.ExecuteAsync(
                id,
                cancellationToken);

            if (product is null)
            {
                return NotFound();
            }

            var response = new ProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Category,
                product.Price);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductResponse>> Update(
        Guid id,
        UpdateProductRequest request,
        CancellationToken cancellationToken)
        {
            var command = new UpdateProductCommand(
                id,
                request.Name,
                request.Description,
                request.Category,
                request.Price);

            var product = await _updateProductUseCase.ExecuteAsync(
                command,
                cancellationToken);

            if (product is null)
            {
                return NotFound();
            }

            var response = new ProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Category,
                product.Price);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
        {
            var deleted = await _deleteProductUseCase.ExecuteAsync(
                id,
                cancellationToken);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
