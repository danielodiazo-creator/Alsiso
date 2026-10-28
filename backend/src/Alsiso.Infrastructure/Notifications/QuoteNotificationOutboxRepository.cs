using Alsiso.Application.Quotes;
using Alsiso.Infrastructure.Products.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Alsiso.Infrastructure.Notifications;

public sealed class QuoteNotificationOutboxRepository(
    AlsisoDbContext dbContext) : IQuoteNotificationOutboxRepository
{
    private const int MaxAttempts = 5;

    public async Task<IReadOnlyList<QuoteNotificationOutboxItem>>
        GetPendingAsync(
            DateTime utcNow,
            int maxItems,
            CancellationToken cancellationToken)
    {
        return await dbContext.QuoteNotificationOutboxMessages
            .AsNoTracking()
            .Where(message =>
                message.ProcessedAtUtc == null &&
                message.NextAttemptAtUtc != null &&
                message.NextAttemptAtUtc <= utcNow &&
                message.Attempts < MaxAttempts)
            .OrderBy(message => message.CreatedAtUtc)
            .Take(maxItems)
            .Select(message => new QuoteNotificationOutboxItem(
                message.Id,
                message.QuoteRequestId,
                message.Attempts))
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsProcessedAsync(
        Guid id,
        DateTime processedAtUtc,
        CancellationToken cancellationToken)
    {
        var message = await FindMessageAsync(id, cancellationToken);
        message.MarkAsProcessed(processedAtUtc);
    }

    public async Task RegisterFailedAttemptAsync(
        Guid id,
        string error,
        DateTime nextAttemptAtUtc,
        CancellationToken cancellationToken)
    {
        var message = await FindMessageAsync(id, cancellationToken);
        message.RegisterFailedAttempt(error, nextAttemptAtUtc);
    }

    private async Task<QuoteNotificationOutboxMessage> FindMessageAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await dbContext.QuoteNotificationOutboxMessages
            .FirstOrDefaultAsync(
                message => message.Id == id,
                cancellationToken)
            ?? throw new InvalidOperationException(
                $"No se encontró el aviso pendiente '{id}'.");
    }
}