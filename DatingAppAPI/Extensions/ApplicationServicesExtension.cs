using Data.Layer.Contexts;
using DatingAppAPI.Middlewares;
using Microsoft.EntityFrameworkCore;
using Repository.Layer;
using Repository.Layer.Interfaces;
using Services.Layer.Account;
using Services.Layer.Identity;
using Services.Layer.Token;

namespace DatingAppAPI.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // 🔹 Add DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddHttpContextAccessor();

            services.AddScoped<ExceptionMiddleware>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<ITokenService, TokenService>();

            // Register the CORS
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:4200",
                    "http://localhost:3000",
                    "https://localhost:3000"); // angular and react
                });
            });

            // 🔹 Register UnitOfWork with AppDbContext
            services.AddScoped(typeof(IUnitOfWork<AppDbContext>), typeof(UnitOfWork<AppDbContext>));

            services.AddScoped<IAccountService, AccountService>(); // Register AccountService as a scoped service>

            // 🔹 Register Services
            //services.AddAutoMapper(typeof(TicketProfile).Assembly);
            return services;
        }
    }
}
