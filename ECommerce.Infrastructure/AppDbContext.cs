using System;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    // ---------- DbSets ----------
    public DbSet<User>        Users         => Set<User>();
    public DbSet<Category>    Categories    => Set<Category>();
    public DbSet<Product>     Products      => Set<Product>();
    public DbSet<Order>       Orders        => Set<Order>();
    public DbSet<OrderItem>   OrderItems    => Set<OrderItem>();
    public DbSet<CartItem>    CartItems     => Set<CartItem>();
    public DbSet<PriceHistory> PriceHistories => Set<PriceHistory>();

    // --- Model Config ---
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // -- User ---
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.HasKey(x => x.Id);

            e.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);

            e.HasIndex(x => x.Email).IsUnique();

            e.Property(x => x.PasswordHash).IsRequired();

            e.Property(x => x.Role)
                .IsRequired()
                .HasMaxLength(30);

            e.Property(x => x.CreatedAt)
                .IsRequired();
        });

        // -Category ---
        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("Categories");
            e.HasKey(x => x.Id);

            e.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            e.HasIndex(x => x.Name).IsUnique();
        });

        // --Product -
        modelBuilder.Entity<Product>(e =>
        {
            e.ToTable("Products");
            e.HasKey(x => x.Id);

            e.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            e.Property(x => x.Sku)
                .IsRequired()
                .HasMaxLength(50);

            // Precio base del producto
            e.Property(x => x.Price)
                .HasPrecision(18, 2);

            // Stock para pricing/inventario. si no existe en tu entidad, agrégalo)
            e.Property(x => x.Stock)
                .HasDefaultValue(0);

            e.HasIndex(x => x.Sku).IsUnique();

            e.HasOne(p => p.Category)
                .WithMany(c => c.Products!)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // -Order --
        modelBuilder.Entity<Order>(e =>
        {
            e.ToTable("Orders");
            e.HasKey(x => x.Id);

            e.Property(x => x.Total)
                .HasPrecision(18, 2);

            // MySQL: datetime(6) 
            e.Property(x => x.CreatedAt)
                .HasColumnType("datetime(6)");

            e.HasMany(x => x.Items)
                .WithOne(i => i.Order!)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // --OrderItem --
        modelBuilder.Entity<OrderItem>(e =>
        {
            e.ToTable("OrderItems");
            e.HasKey(x => x.Id);

            e.Property(x => x.UnitPrice)
                .HasPrecision(18, 2);

            e.Property(x => x.Quantity)
                .HasDefaultValue(1);
        });

        // -CartItem --
        modelBuilder.Entity<CartItem>(e =>
        {
            e.ToTable("CartItems");
            e.HasKey(x => x.Id);

            e.Property(x => x.Quantity)
                .HasDefaultValue(1);

            e.HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Un ítem por combinación (UserId, ProductId)
            e.HasIndex(x => new { x.UserId, x.ProductId }).IsUnique();
        });

        // - PriceHistory -
        modelBuilder.Entity<PriceHistory>(e =>
        {
            e.ToTable("PriceHistories");
            e.HasKey(x => x.Id);

            e.Property(x => x.OldPrice).HasPrecision(18, 2);
            e.Property(x => x.NewPrice).HasPrecision(18, 2);

            e.Property(x => x.AppliedAt).HasColumnType("datetime(6)");

            e.HasOne(ph => ph.Product)
                .WithMany()
                .HasForeignKey(ph => ph.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }


    Task<int> IAppDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        => base.SaveChangesAsync(cancellationToken);
}
