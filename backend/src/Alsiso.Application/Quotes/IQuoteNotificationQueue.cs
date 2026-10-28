namespace Alsiso.Application.Quotes;

public interface IQuoteNotificationQueue
{
    Task EnqueueNewQuoteNotificationAsync(
        Guid quoteRequestId,
        CancellationToken cancellationToken);
}