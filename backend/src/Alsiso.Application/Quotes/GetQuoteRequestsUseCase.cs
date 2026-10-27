using Alsiso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Quotes
{
    public sealed class GetQuoteRequestsUseCase(
        IQuoteRequestRepository quoteRequestRepository)
    {
        public async Task<IReadOnlyList<QuoteRequest>> ExecuteAsync(
           CancellationToken cancellationToken)
        {
            return await quoteRequestRepository.GetAllAsync(
                cancellationToken);
        }


    }
}
