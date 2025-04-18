using Data.Layer.Contexts;
using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Data.Layer
{
    public class UsersContextSeed
    {
        public static async Task Seed(AppDbContext context, UserManager<AppUser> userManager, ILoggerFactory loggerFactory)
        {
            if (await context.Database.CanConnectAsync())
            {
                try
                {
                    // Only seed if no users exist
                    if (!userManager.Users.Any())
                    {
                        var users = GetUsers();

                        foreach (var user in users)
                        {
                            var result = await userManager.CreateAsync(user, "P@ssw0rd!");

                            if (!result.Succeeded)
                            {
                                throw new Exception($"Failed to create user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<UsersContextSeed>();
                    logger.LogError(ex, "An error occurred while seeding the users.");
                }
            }
        }

        public static List<AppUser> GetUsers()
        {
            return new List<AppUser>
        {
            new AppUser
            {
                UserName = "ahmed123",
                Email = "ahmed@example.com",
                DisplayName = "Ahmed Gomaa",
                Bio = "Backend .NET Developer",
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Cairo",
                    State = "C",
                    ZipCode = "11311"
                },
                EmailConfirmed = true
            },
            new AppUser
            {
                UserName = "sara88",
                Email = "sara@example.com",
                DisplayName = "Sara H.",
                Bio = "UI/UX Designer",
                Address = new Address
                {
                    Street = "456 Nile St",
                    City = "Giza",
                    State = "G",
                    ZipCode = "11512"
                },
                EmailConfirmed = true
            },
            new AppUser
            {
                UserName = "mohamed99",
                Email = "mohamed@example.com",
                DisplayName = "Mohamed M.",
                Bio = "Frontend Developer",
                Address = new Address
                {
                    Street = "789 Nile St",
                    City = "Cairo",
                    State = "C",
                    ZipCode = "11311"
                },
                EmailConfirmed = true
            },
            new AppUser
            {
                UserName = "khalid77",
                Email = "khalid@example.com",
                DisplayName = "Khalid A.",
                Bio = "Backend .NET Developer",
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Cairo",
                    State = "C",
                    ZipCode = "11311"
                },
                EmailConfirmed = true
            },
            new AppUser
            {
                UserName = "yasser55",
                Email = "yasser@example.com",
                DisplayName = "Yasser E.",
                Bio = "UI/UX Designer",
                Address = new Address
                {
                    Street = "456 Nile St",
                    City = "Giza",
                    State = "G",
                    ZipCode = "11512"
                },
                EmailConfirmed = true
            }
        };
        }
    }
}
