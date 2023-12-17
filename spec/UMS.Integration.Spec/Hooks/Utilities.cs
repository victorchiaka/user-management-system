using UMS.Features;
using UMS.Persistence;

namespace UMS.Integration.Spec.Hooks;

public static class Utilities
{
    public static void SeedData(UserDbContext db)
    {
        var users = new[]
        {
            new User
            {
                Username = "victor",
                EmailAddress = "victor@mail.com",
                PasswordHash =  BCrypt.Net.BCrypt.HashPassword("password"),
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Username = "ben10",
                EmailAddress = "ben@mail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                CreatedAt = DateTime.UtcNow
            }
        };

        db.Users.AddRange(users);
        db.SaveChanges();
    }
}