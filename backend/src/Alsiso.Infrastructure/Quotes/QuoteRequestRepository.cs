using Alsiso.Application.Quotes;
using Alsiso.Domain.Entities;
using Alsiso.Infrastructure.Products.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Infrastructure.Quotes
{
    public sealed class QuoteRequestRepository(
     AlsisoDbContext dbContext) : IQuoteRequestRepository
    {
        public async Task AddAsync(
            QuoteRequest quoteRequest,
            CancellationToken cancellationToken)
        {
            await dbContext.QuoteRequests.AddAsync(
                quoteRequest,
                cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
