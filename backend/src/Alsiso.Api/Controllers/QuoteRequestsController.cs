using Alsiso.Api.Dtos;
using Alsiso.Application.Quotes;
using Microsoft.AspNetCore.Mvc;

namespace Alsiso.Api.Controllers;

[ApiController]
[Route("api/quote-requests")]
public sealed class QuoteRequestsController(
    CreateQuoteRequestUseCase createQuoteRequestUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<QuoteRequestResponse>> Create(
        CreateQuoteRequestApiRequest request,
        CancellationToken cancellationToken)
    {
        if (request.ProductId == Guid.Empty)
        {
            return BadRequest(new
            {
                message = "Debes seleccionar un producto."
            });
        }

        var command = new CreateQuoteRequestCommand(
            request.ProductId,
            request.CustomerName,
            request.Email,
            request.Phone,
            request.Quantity,
            request.Message);

        var quoteRequest = await createQuoteRequestUseCase.ExecuteAsync(
            command,
            cancellationToken);

        if (quoteRequest is null)
        {
            return NotFound(new
            {
                message = "El producto seleccionado no existe."
            });
        }

        var response = new QuoteRequestResponse(
            quoteRequest.Id,
            quoteRequest.ProductId,
            quoteRequest.ProductName,
            quoteRequest.CustomerName,
            quoteRequest.Email,
            quoteRequest.Phone,
            quoteRequest.Quantity,
            quoteRequest.Message,
            quoteRequest.CreatedAt);

        return StatusCode(StatusCodes.Status201Created, response);
    }
}