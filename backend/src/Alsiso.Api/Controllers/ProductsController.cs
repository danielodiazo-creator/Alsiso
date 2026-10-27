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

        private const long MaxImageSizeBytes = 5 * 1024 * 1024;
        private readonly UploadProductImageUseCase _uploadProductImageUseCase;

        private string? BuildImageUrl(string? imageReference)
        {
            if (string.IsNullOrWhiteSpace(imageReference))
            {
                return null;
            }

            if (Uri.TryCreate(imageReference, UriKind.Absolute, out _))
            {
                return imageReference;
            }

            return $"{Request.Scheme}://{Request.Host}{imageReference}";
        }

        private readonly RemoveProductImageUseCase _removeProductImageUseCase;

        public ProductsController(
        ListProductsUseCase listProductsUseCase,
        CreateProductUseCase createProductUseCase,
        GetProductByIdUseCase getProductByIdUseCase,
        UpdateProductUseCase updateProductUseCase,
        DeleteProductUseCase deleteProductUseCase,
        UploadProductImageUseCase uploadProductImageUseCase,
        RemoveProductImageUseCase removeProductImageUseCase)
        {
            _listProductsUseCase = listProductsUseCase;
            _createProductUseCase = createProductUseCase;
            _getProductByIdUseCase = getProductByIdUseCase;
            _updateProductUseCase = updateProductUseCase;
            _deleteProductUseCase = deleteProductUseCase;
            _uploadProductImageUseCase = uploadProductImageUseCase;
            _removeProductImageUseCase = removeProductImageUseCase;
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
                    product.Price,
                    BuildImageUrl(product.ImageUrl)))

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
                product.Price,
                BuildImageUrl(product.ImageUrl));

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
                product.Price,
                BuildImageUrl(product.ImageUrl));

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
                product.Price,
                BuildImageUrl(product.ImageUrl));

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

        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        [HttpPost("{id:guid}/image")]
        [RequestSizeLimit(MaxImageSizeBytes + 64 * 1024)]
        public async Task<ActionResult<ProductResponse>> UploadImage(
        Guid id,
        IFormFile image,
        CancellationToken cancellationToken)
        {
            if (image is null || image.Length == 0)
            {
                return BadRequest(new
                {
                    message = "Debes seleccionar una imagen."
                });
            }

            if (image.Length > MaxImageSizeBytes)
            {
                return BadRequest(new
                {
                    message = "La imagen no puede superar los 5 MB."
                });
            }

            if (!string.Equals(
                    Path.GetExtension(image.FileName),
                    ".png",
                    StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(
                    image.ContentType,
                    "image/png",
                    StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new
                {
                    message = "Solo se permiten imágenes PNG."
                });
            }

            await using var imageStream = new MemoryStream();
            await image.CopyToAsync(imageStream, cancellationToken);
            imageStream.Position = 0;

            var pngSignature = new byte[]
            {
             137, 80, 78, 71, 13, 10, 26, 10
            };
            var fileHeader = new byte[pngSignature.Length];

            var bytesRead = await imageStream.ReadAsync(
                fileHeader.AsMemory(),
                cancellationToken);

            if (bytesRead != pngSignature.Length ||
                !fileHeader.SequenceEqual(pngSignature))
            {
                return BadRequest(new
                {
                    message = "El archivo seleccionado no es un PNG válido."
                });
            }

            imageStream.Position = 0;

            var command = new UploadProductImageCommand(
                id,
                imageStream,
                image.ContentType);

            var product = await _uploadProductImageUseCase.ExecuteAsync(
                command,
                cancellationToken);

            if (product is null)
            {
                return NotFound(new
                {
                    message = "El producto seleccionado no existe."
                });
            }

            var response = new ProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Category,
                product.Price,
                BuildImageUrl(product.ImageUrl));

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}/image")]
        public async Task<ActionResult<ProductResponse>> RemoveImage(
        Guid id,
        CancellationToken cancellationToken)
        {
            var product = await _removeProductImageUseCase.ExecuteAsync(
                id,
                cancellationToken);

            if (product is null)
            {
                return NotFound(new
                {
                    message = "El producto seleccionado no existe."
                });
            }

            var response = new ProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Category,
                product.Price,
                BuildImageUrl(product.ImageUrl));

            return Ok(response);
        }

    }
}
