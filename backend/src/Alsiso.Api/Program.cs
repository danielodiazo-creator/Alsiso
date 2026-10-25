
using Alsiso.Application.Products;
using Alsiso.Infrastructure.Products;
using Alsiso.Infrastructure.Products.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Alsiso.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddScoped<IProductRepository, 
                ProductRepository>();
            builder.Services.AddScoped<ListProductsUseCase>();
            builder.Services.AddScoped<CreateProductUseCase>();
            builder.Services.AddScoped<GetProductByIdUseCase>();




            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            var connectionString = 
            builder.Configuration.GetConnectionString("Alsiso");
            builder.Services.AddDbContext<AlsisoDbContext>(options =>
            options.UseSqlite(connectionString));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "Alsiso API v1");
                });



            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
