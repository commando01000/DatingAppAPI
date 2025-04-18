using Data.Layer.Contexts;
using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DatingAppAPI.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Identity with the AppUser and IdentityRole types
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // Configure Identity options (optional)
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                options.User.RequireUniqueEmail = true; // Ensure emails are unique
            })
            .AddEntityFrameworkStores<AppDbContext>() // Use AppDbContext for Identity
            .AddDefaultTokenProviders(); // Add default token providers (e.g., for email confirmation)

            // Add JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Google:ClientId"];
                options.ClientSecret = configuration["Google:ClientSecret"];
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"])),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Token:Issuer"],
                    ValidateAudience = false,
                    ValidAudience = configuration["Token:Audience"],
                    ValidateLifetime = true, // Validate token expiration
                    ClockSkew = TimeSpan.Zero // No tolerance for expiration time
                };

                // Optional: Add event handlers for JWT authentication
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                        logger.LogError(context.Exception, "JWT authentication failed.");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        // You can add custom logic here when the token is validated
                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }
    }
}
