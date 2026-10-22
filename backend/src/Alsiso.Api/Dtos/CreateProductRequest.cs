namespace Alsiso.Api.Dtos
{
    public sealed record CreateProductRequest(
    string Name,
    string Description,
    string Category,
    decimal? Price);
}
