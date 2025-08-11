using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Abstractions
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Category> Categories { get; }
        DbSet<Product> Products { get; }
        DbSet<CartItem> CartItems { get; }
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }

       
        DbSet<PriceHistory> PriceHistories { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
