using Alsiso.Domain.Enums;

namespace Alsiso.Api.Dtos
{
    public sealed record QuoteRequestResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string CustomerName,
    string Email,
    string? Phone,
    int Quantity,
    string? Message,
    DateTimeOffset CreatedAt,
    QuoteRequestStatus Status);
}
