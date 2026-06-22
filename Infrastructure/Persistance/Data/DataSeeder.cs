// Infrastructure/Persistance/Data/DataSeeder.cs
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;

namespace Infrastructure.Persistance.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.Add(new User
            {
                FullName = "Admin",
                Email = "admin@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }
    }
}