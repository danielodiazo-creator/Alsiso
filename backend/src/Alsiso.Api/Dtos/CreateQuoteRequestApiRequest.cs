using System.ComponentModel.DataAnnotations;

namespace Alsiso.Api.Dtos;

public sealed record CreateQuoteRequestApiRequest(
    Guid ProductId,
    [Required, StringLength(100)] string CustomerName,
    [Required, EmailAddress, StringLength(254)] string Email,
    [Phone, StringLength(30)] string? Phone,
    [Range(1, 100000)] int Quantity,
    [StringLength(2000)] string? Message);