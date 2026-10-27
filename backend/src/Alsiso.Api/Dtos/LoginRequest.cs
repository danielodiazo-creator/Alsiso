using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace Alsiso.Api.Dtos
{
    public sealed record LoginRequest(
        [Required, EmailAddress] string Email,
        [Required] string Password);
    
    
}
