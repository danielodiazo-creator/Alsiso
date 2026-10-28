using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Quotes;

public interface IQuoteNotificationOutboxRepository
{
    Task<IReadOnlyList<QuoteNotificationOutboxItem>> GetPendingAsync(
        DateTime utcNow,
        int maxItems,
        CancellationToken cancellationToken);

    Task MarkAsProcessedAsync(
        Guid id,
        DateTime processedAtUtc,
        CancellationToken cancellationToken);

    Task RegisterFailedAttemptAsync(
        Guid id,
        string error,
        DateTime nextAttemptAtUtc,
        CancellationToken cancellationToken);
}