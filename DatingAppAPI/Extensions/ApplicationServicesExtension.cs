using Data.Layer.Contexts;
using DatingAppAPI.Middlewares;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Layer;
using Repository.Layer.Interfaces;
using Services.Layer;
using Services.Layer;
using Services.Layer.Helpers;
using Services.Layer.Identity;
using Services.Layer.Member;
using Services.Layer;
using Services.Layer.Profiles;
using Services.Layer.Token;
using Services.Layer.UserLikes;

namespace DatingAppAPI.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // 🔹 Add DbContext
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
            config.GetConnectionString("DefaultConnection"),
            sqlOptions => sqlOptions
            .MigrationsAssembly("Data.Layer")
            ));


            services.AddHttpContextAccessor();

            services.AddScoped<ExceptionMiddleware>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IUserLikeService, UserLikeService>();


            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            // Register the CORS
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:4200",
                    "https://localhost:7120",
                    "http://localhost:7120",
                    "http://localhost:3000",
                    "https://supraa-dating-app.runasp.net",
                    "https://localhost:3000"); // angular and react
                });
            });

            // 🔹 Register UnitOfWork with AppDbContext
            services.AddScoped(typeof(IUnitOfWork<AppDbContext>), typeof(UnitOfWork<AppDbContext>));
            //services.AddScoped(typeof(IUnitOfWork<IdentityDbContext>), typeof(UnitOfWork<IdentityDbContext>));

            services.AddScoped<IAccountService, AccountService>(); // Register AccountService as a scoped service>
            services.AddScoped<IPhotoService, PhotoService>(); // Register PhotoService as a scoped service>
            services.AddScoped<IMemberService, MemberService>(); // Register AccountService as a scoped service>

            // Register AutoMappers
            services.AddAutoMapper(typeof(UserProfile).Assembly);

            // 🔹 Register Services
            //services.AddAutoMapper(typeof(TicketProfile).Assembly);
            return services;
        }
    }
}
