using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Infrastructure.Notifications;

public sealed class QuoteNotificationOutboxMessage
{
    private QuoteNotificationOutboxMessage()
    {
    }

    public Guid Id { get; private set; }
    public Guid QuoteRequestId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? ProcessedAtUtc { get; private set; }
    public DateTime? NextAttemptAtUtc { get; private set; }
    public int Attempts { get; private set; }
    public string? LastError { get; private set; }

    public QuoteNotificationOutboxMessage(
        Guid quoteRequestId,
        DateTime createdAtUtc)
    {
        if (quoteRequestId == Guid.Empty)
        {
            throw new ArgumentException(
                "El identificador de la cotización es obligatorio.",
                nameof(quoteRequestId));
        }

        Id = Guid.NewGuid();
        QuoteRequestId = quoteRequestId;
        CreatedAtUtc = createdAtUtc;
        NextAttemptAtUtc = createdAtUtc;
    }

    public void MarkAsProcessed(DateTime processedAtUtc)
    {
        ProcessedAtUtc = processedAtUtc;
        NextAttemptAtUtc = null;
        LastError = null;
    }

    public void RegisterFailedAttempt(
        string error,
        DateTime nextAttemptAtUtc)
    {
        Attempts++;
        LastError = error;
        NextAttemptAtUtc = nextAttemptAtUtc;
    }
}
