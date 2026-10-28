using Alsiso.Application.Abstractions;
using Alsiso.Domain.Entities;

namespace Alsiso.Application.Quotes;

public sealed class ProcessPendingQuoteNotificationsUseCase(
    IQuoteNotificationOutboxRepository outboxRepository,
    IQuoteRequestRepository quoteRequestRepository,
    IQuoteRequestNotifier quoteRequestNotifier,
    IUnitOfWork unitOfWork)
{
    private const int BatchSize = 10;
    private const int MaximumRetryDelayMinutes = 60;

    public async Task<int> ExecuteAsync(
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var pendingMessages = await outboxRepository.GetPendingAsync(
            now,
            BatchSize,
            cancellationToken);

        foreach (var message in pendingMessages)
        {
            try
            {
                var quoteRequest =
                    await quoteRequestRepository.GetByIdAsync(
                        message.QuoteRequestId,
                        cancellationToken);

                if (quoteRequest is null)
                {
                    throw new InvalidOperationException(
                        $"No se encontró la cotización '{message.QuoteRequestId}'.");
                }

                await quoteRequestNotifier.NotifyNewQuoteAsync(
                    quoteRequest,
                    cancellationToken);

                await outboxRepository.MarkAsProcessedAsync(
                    message.Id,
                    DateTime.UtcNow,
                    cancellationToken);
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                var retryDelayMinutes = Math.Min(
                    Math.Pow(2, message.Attempts),
                    MaximumRetryDelayMinutes);

                var nextAttemptAtUtc = DateTime.UtcNow.AddMinutes(
                    retryDelayMinutes);

                await outboxRepository.RegisterFailedAttemptAsync(
                    message.Id,
                    exception.Message,
                    nextAttemptAtUtc,
                    cancellationToken);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return pendingMessages.Count;
    }
}