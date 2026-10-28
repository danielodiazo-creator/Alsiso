using Alsiso.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Quotes
{
    public interface IQuoteRequestNotifier
    {
        Task NotifyNewQuoteAsync(
        QuoteRequest quoteRequest,
        CancellationToken cancellationToken);

    }
}
