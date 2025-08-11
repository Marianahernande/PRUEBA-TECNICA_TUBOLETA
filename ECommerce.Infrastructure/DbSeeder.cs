using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace ECommerce.Infrastructure;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        if (!await db.Categories.AnyAsync())
        {
            var cat = new Category { Name = "General" };
            db.Categories.Add(cat);

            db.Products.AddRange(
                new Product { Name = "Camiseta", Sku = "SKU-001", Price = 19.99m, Category = cat },
                new Product { Name = "Gorra", Sku = "SKU-002", Price = 12.50m, Category = cat }
            );

            await db.SaveChangesAsync();
        }
        
        if (!await db.Users.AnyAsync(u => u.Email == "admin@demo.com"))
{
    db.Users.Add(new User {
        Email = "admin@demo.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
        Role = "Admin"
    });
    await db.SaveChangesAsync();
}
    }
}
