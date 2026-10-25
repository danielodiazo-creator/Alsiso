using System.ComponentModel.DataAnnotations;

namespace Alsiso.Api.Dtos
{

    public sealed record UpdateProductRequest(
        [Required] string Name,
        string Description,
        [Required] string Category,
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        decimal? Price);
}
