using Microsoft.OpenApi.Models;

namespace DatingAppAPI.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Dating App API",
                    Version = "v1",
                    Description = "API for Dating App",
                    Contact = new OpenApiContact
                    {
                        Name = "Supraa",
                        Email = "jfijcc124@gmail.com",
                        Url = new Uri(configuration["BaseUrl"])
                    }
                });
                options.AddServer(new OpenApiServer
                {
                    Url = configuration["BaseUrl"],
                    Description = "Dating App Server"
                });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme,
                    }
                };
                options.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };
                options.AddSecurityRequirement(securityRequirement);
            });
            return services;
        }
    }
}
