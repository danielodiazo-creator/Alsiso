
using Alsiso.Application.Products;
using Alsiso.Infrastructure.Products;
using Alsiso.Infrastructure.Products.Persistence;
using Microsoft.EntityFrameworkCore;
using Alsiso.Application.Quotes;
using Alsiso.Infrastructure.Quotes;
using Microsoft.AspNetCore.Identity;
using Alsiso.Infrastructure.Products.Images;

namespace Alsiso.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddScoped<IProductRepository, 
                ProductRepository>();
            builder.Services.AddScoped<ListProductsUseCase>();
            builder.Services.AddScoped<CreateProductUseCase>();
            builder.Services.AddScoped<GetProductByIdUseCase>();
            builder.Services.AddScoped<UpdateProductUseCase>();
            builder.Services.AddScoped<DeleteProductUseCase>();
            
            builder.Services.AddScoped<
            IQuoteRequestRepository,
            QuoteRequestRepository>();
            
            builder.Services.AddScoped<
            IQuoteRequestNotifier,
            SmtpQuoteRequestNotifier>();
           
            builder.Services.AddScoped<CreateQuoteRequestUseCase>();
            builder.Services.AddScoped<GetQuoteRequestsUseCase>();

            var productImagesDirectory = Path.Combine(
            builder.Environment.ContentRootPath,
            "wwwroot",
            "uploads",
            "products");

            builder.Services.AddScoped<IProductImageStorage>(
                _ => new LocalProductImageStorage(productImagesDirectory));

            builder.Services.AddScoped<UploadProductImageUseCase>();
            builder.Services.AddScoped<RemoveProductImageUseCase>();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendDev", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            builder.Services
           .AddIdentity<IdentityUser, IdentityRole>()
           .AddEntityFrameworkStores<AlsisoDbContext>()
           .AddDefaultTokenProviders();

            builder.Services.AddAuthorization();



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
                await SeedAdminAsync(app);
                app.MapOpenApi();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "Alsiso API v1");
                });



            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("FrontendDev");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static async Task SeedAdminAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            var email = configuration["Admin:Email"];
            var password = configuration["Admin:Password"];

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    "Faltan Admin:Email o Admin:Password en User Secrets.");
            }

            const string adminRole = "Admin";

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                var roleResult = await roleManager.CreateAsync(
                    new IdentityRole(adminRole));

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        roleResult.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"No se pudo crear el rol Admin: {errors}");
                }
            }

            var adminUser = await userManager.FindByEmailAsync(email);

            if (adminUser is null)
            {
                adminUser = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var userResult = await userManager.CreateAsync(adminUser, password);

                if (!userResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        userResult.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"No se pudo crear la cuenta de administrador: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser, adminRole))
            {
                var roleAssignmentResult = await userManager.AddToRoleAsync(
                    adminUser,
                    adminRole);

                if (!roleAssignmentResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        roleAssignmentResult.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"No se pudo asignar el rol Admin: {errors}");
                }
            }
        }
    }
}
