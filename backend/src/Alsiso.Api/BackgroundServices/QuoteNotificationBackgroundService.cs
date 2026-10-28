using Alsiso.Application.Quotes;

namespace Alsiso.Api.BackgroundServices;

public sealed class QuoteNotificationBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<QuoteNotificationBackgroundService> logger)
    : BackgroundService
{
    private static readonly TimeSpan PollingInterval =
        TimeSpan.FromSeconds(30);

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var processor = scope.ServiceProvider
                    .GetRequiredService<
                        ProcessPendingQuoteNotificationsUseCase>();

                var processedCount = await processor.ExecuteAsync(
                    stoppingToken);

                if (processedCount > 0)
                {
                    logger.LogInformation(
                        "Se procesaron {Count} avisos de cotización.",
                        processedCount);
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "Ocurrió un error al procesar avisos de cotización.");
            }

            await Task.Delay(PollingInterval, stoppingToken);
        }
    }
}