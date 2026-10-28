using Alsiso.Application.Quotes;
using Alsiso.Domain.Entities;
using Alsiso.Infrastructure.Products.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

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

        }

        public async Task<IReadOnlyList<QuoteRequest>> GetAllAsync(
        CancellationToken cancellationToken)
        {
            var quoteRequests = await dbContext.QuoteRequests
           .AsNoTracking()
           .ToListAsync(cancellationToken);

            return quoteRequests
                .OrderByDescending(quoteRequest => quoteRequest.CreatedAt)
                .ToList();
        }

        public async Task<QuoteRequest?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
        {
            return await dbContext.QuoteRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    quoteRequest => quoteRequest.Id == id,
                    cancellationToken);
        }


        public async Task<QuoteRequest?> GetTrackedByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
        {
            return await dbContext.QuoteRequests
                .FirstOrDefaultAsync(
                    quoteRequest => quoteRequest.Id == id,
                    cancellationToken);
        }



    }


}
