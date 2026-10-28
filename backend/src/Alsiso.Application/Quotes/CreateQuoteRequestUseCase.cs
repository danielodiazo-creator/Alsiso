using Alsiso.Application.Abstractions;
using Alsiso.Application.Products;
using Alsiso.Domain.Entities;

namespace Alsiso.Application.Quotes;

public sealed class CreateQuoteRequestUseCase(
    IProductRepository productRepository,
    IQuoteRequestRepository quoteRequestRepository,
    IQuoteNotificationQueue quoteNotificationQueue,
    IUnitOfWork unitOfWork)
{
    public async Task<QuoteRequest?> ExecuteAsync(
        CreateQuoteRequestCommand command,
        CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(
            command.ProductId,
            cancellationToken);

        if (product is null)
        {
            return null;
        }

        var quoteRequest = new QuoteRequest(
            Guid.NewGuid(),
            product.Id,
            product.Name,
            command.CustomerName,
            command.Email,
            command.Phone,
            command.Quantity,
            command.Message,
            DateTimeOffset.UtcNow);

        await quoteRequestRepository.AddAsync(
            quoteRequest,
            cancellationToken);

        await quoteNotificationQueue.EnqueueNewQuoteNotificationAsync(
            quoteRequest.Id,
            cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return quoteRequest;
    }
}