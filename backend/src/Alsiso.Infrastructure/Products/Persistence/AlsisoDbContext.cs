using Alsiso.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsiso.Infrastructure.Products.Persistence
{
    public sealed class AlsisoDbContext : DbContext
    {
        public AlsisoDbContext(DbContextOptions<AlsisoDbContext>options)
            : base(options) 
        { }

        public DbSet<Product> Products => Set<Product>();

        public DbSet<QuoteRequest> QuoteRequests => Set<QuoteRequest>();
    }
}
