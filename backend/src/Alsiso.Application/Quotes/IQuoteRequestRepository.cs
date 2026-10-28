using Alsiso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Quotes
{
    public interface IQuoteRequestRepository
    {
        Task AddAsync(
            QuoteRequest quoteRequest,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<QuoteRequest>> GetAllAsync(
        CancellationToken cancellationToken);

        Task<QuoteRequest?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);
    }
}
