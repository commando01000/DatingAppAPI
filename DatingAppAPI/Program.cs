
using Data.Layer;
using Data.Layer.Contexts;
using Data.Layer.Entities.Identity;
using DatingAppAPI.Extensions;
using DatingAppAPI.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddSwaggerServices(builder.Configuration);

            var app = builder.Build();

            // Seed the database and apply pending migrations if needed
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var LoggerFactory = services.GetRequiredService<ILoggerFactory>();
                var UserManager = services.GetRequiredService<UserManager<AppUser>>();
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();

                    // Apply pending migrations
                    await context.Database.MigrateAsync();

                    // Seed the database
                    await UsersContextSeed.Seed(context, UserManager, LoggerFactory);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            // Register the middleware
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseAuthentication(); // Ensure this comes before Use Authorization
            app.UseAuthorization();
            app.MapControllers();
            
            app.Run();
        }
    }
}
