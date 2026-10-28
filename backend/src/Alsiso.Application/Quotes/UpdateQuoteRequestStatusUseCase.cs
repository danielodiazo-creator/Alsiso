using Alsiso.Application.Abstractions;
using Alsiso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Quotes
{
    public sealed class UpdateQuoteRequestStatusUseCase(
        IQuoteRequestRepository quoteRequestRepository,
        IUnitOfWork unitOfWork)
    {
        public async Task<QuoteRequest?> ExecuteAsync(
            UpdateQuoteRequestStatusCommand command,
            CancellationToken cancellationToken)
        {
            var quoteRequest = await quoteRequestRepository.GetTrackedByIdAsync(
                command.QuoteRequestId,
                cancellationToken);

            if (quoteRequest is null)
            {
                return null;
            }

            quoteRequest.ChangeStatus(command.Status);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return quoteRequest;
        }
    }
}