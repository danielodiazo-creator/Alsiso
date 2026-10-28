using Alsiso.Application.Quotes;
using Alsiso.Infrastructure.Products.Persistence;

namespace Alsiso.Infrastructure.Notifications;

public sealed class QuoteNotificationQueue(
    AlsisoDbContext dbContext) : IQuoteNotificationQueue
{
    public async Task EnqueueNewQuoteNotificationAsync(
        Guid quoteRequestId,
        CancellationToken cancellationToken)
    {
        var outboxMessage = new QuoteNotificationOutboxMessage(
            quoteRequestId,
            DateTime.UtcNow);

        await dbContext.QuoteNotificationOutboxMessages.AddAsync(
            outboxMessage,
            cancellationToken);
    }
}