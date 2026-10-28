using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Quotes;

public sealed record QuoteNotificationOutboxItem(
    Guid Id,
    Guid QuoteRequestId,
    int Attempts);