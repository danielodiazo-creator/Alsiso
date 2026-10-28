using Alsiso.Domain.Enums;

namespace Alsiso.Api.Dtos
{
    public sealed record UpdateQuoteRequestStatusApiRequest(
     QuoteRequestStatus Status);
}
