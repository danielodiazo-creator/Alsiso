using System;
using System.Collections.Generic;
using System.Text;
using Alsiso.Domain.Enums;

namespace Alsiso.Application.Quotes;

public sealed record UpdateQuoteRequestStatusCommand(
    Guid QuoteRequestId,
    QuoteRequestStatus Status);