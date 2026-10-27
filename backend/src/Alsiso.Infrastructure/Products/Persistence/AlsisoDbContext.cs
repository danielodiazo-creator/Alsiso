using Alsiso.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Alsiso.Infrastructure.Products.Persistence
{
    public sealed class AlsisoDbContext : IdentityDbContext<IdentityUser>
    {
        public AlsisoDbContext(
            DbContextOptions<AlsisoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();

        public DbSet<QuoteRequest> QuoteRequests => Set<QuoteRequest>();
    }
}