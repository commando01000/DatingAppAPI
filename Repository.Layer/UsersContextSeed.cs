using Data.Layer.Contexts;
using Data.Layer.Entities.Identity;
using Data.Layer.Helpers;
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
            Gender = "Male",
            Interests = "Clean architecture, APIs, reading tech blogs",
            PhoneNumber = "+201234567896",
            LookingFor = "Project collaboration",
            DateOfBirth = new DateOnly(1993, 5, 22),
            Age = Extensions.CalculateAge(new DateOnly(1993, 5, 22)),
            LastActive = DateTime.UtcNow.AddDays(-2),
            Address = new Address
            {
                Street = "123 Main St",
                City = "Cairo",
                State = "C",
                ZipCode = "11311"
            },
            Photos = new List<Photo>
            {
                new Photo { Url = "https://picsum.photos/200/300?1", IsMain = true, PublicId = "ahmed_photo" }
            },
            EmailConfirmed = true
        },
        new AppUser
        {
            UserName = "sara88",
            Email = "sara@example.com",
            DisplayName = "Sara H.",
            Bio = "UI/UX Designer",
            Gender = "Female",
            Interests = "Design systems, Figma, traveling",
            LookingFor = "Design team role",
            PhoneNumber = "+20122251890",
            DateOfBirth = new DateOnly(1990, 10, 14),
            Age = Extensions.CalculateAge(new DateOnly(1990, 10, 14)),
            LastActive = DateTime.UtcNow.AddDays(-5),
            Address = new Address
            {
                Street = "456 Nile St",
                City = "Giza",
                State = "G",
                ZipCode = "11512"
            },
            Photos = new List<Photo>
            {
                new Photo { Url = "https://picsum.photos/200/300?2", IsMain = true, PublicId = "sara_photo" }
            },
            EmailConfirmed = true
        },
        new AppUser
        {
            UserName = "mohamed99",
            Email = "mohamed@example.com",
            DisplayName = "Mohamed M.",
            Bio = "Frontend Developer",
            Gender = "Male",
            Interests = "Angular, JavaScript, UI performance",
            LookingFor = "Mentorship",
            PhoneNumber = "+201234567820",
            DateOfBirth = new DateOnly(1995, 7, 9),
            Age = Extensions.CalculateAge(new DateOnly(1995, 7, 9)),
            LastActive = DateTime.UtcNow.AddDays(-1),
            Address = new Address
            {
                Street = "789 Nile St",
                City = "Cairo",
                State = "C",
                ZipCode = "11311"
            },
            Photos = new List<Photo>
            {
                new Photo { Url = "https://picsum.photos/200/300?3", IsMain = true, PublicId = "mohamed_photo" }
            },
            EmailConfirmed = true
        },
        new AppUser
        {
            UserName = "khalid77",
            Email = "khalid@example.com",
            DisplayName = "Khalid A.",
            Bio = "Backend .NET Developer",
            Gender = "Male",
            Interests = ".NET Core, microservices, cloud",
            LookingFor = "Remote backend roles",
            DateOfBirth = new DateOnly(1988, 3, 11),
            PhoneNumber = "+201234562291",
            Age = Extensions.CalculateAge(new DateOnly(1988, 3, 11)),
            LastActive = DateTime.UtcNow.AddDays(-7),
            Address = new Address
            {
                Street = "123 Main St",
                City = "Cairo",
                State = "C",
                ZipCode = "11311"
            },
            Photos = new List<Photo>
            {
                new Photo { Url = "https://picsum.photos/200/300?4", IsMain = true, PublicId = "khalid_photo" }
            },
            EmailConfirmed = true
        },
        new AppUser
        {
            UserName = "yasser55",
            Email = "yasser@example.com",
            DisplayName = "Yasser E.",
            Bio = "UI/UX Designer",
            Gender = "Male",
            PhoneNumber = "+201133367890",
            Interests = "User research, design psychology",
            LookingFor = "Startup teams",
            DateOfBirth = new DateOnly(1991, 1, 30),
            Age = Extensions.CalculateAge(new DateOnly(1991, 1, 30)),
            LastActive = DateTime.UtcNow.AddDays(-3),
            Address = new Address
            {
                Street = "456 Nile St",
                City = "Giza",
                State = "G",
                ZipCode = "11512"
            },
            Photos = new List<Photo>
            {
                new Photo { Url = "https://picsum.photos/200/300?5", IsMain = true, PublicId = "yasser_photo" }
            },
            EmailConfirmed = true
        }
    };
        }

    }
}
