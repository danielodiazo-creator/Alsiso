using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Application.Quotes
{
    public sealed record CreateQuoteRequestCommand(
    Guid ProductId,
    string CustomerName,
    string Email,
    string? Phone,
    int Quantity,
    string? Message);
}
