namespace Alsiso.Api.Dtos
{
    public sealed record ProductResponse(
        Guid Id,
        string Name,
        string Description,
        string Category,
        decimal? Price,
        string? ImageUrl);
    
        



    
}
