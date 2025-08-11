using Catalog.Api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core (MySQL)
var cs = builder.Configuration.GetConnectionString("Default")
         ?? "Server=localhost;Port=3306;Database=ecommerce;User=ecommerce;Password=ecommerce123;";
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySql(cs, ServerVersion.AutoDetect(cs)));

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Endpoints !!!
app.MapGet("/api/products", async (AppDbContext db) =>
    await db.Products.AsNoTracking().Select(p => new { p.Id, p.Sku, p.Name, p.Price, p.CategoryId }).ToListAsync());

app.MapGet("/api/categories", async (AppDbContext db) =>
    await db.Categories.AsNoTracking().ToListAsync());


app.MapGet("/healthz", () => Results.Ok("ok"));

// Seeder simple (si no hay categorías)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    if (!db.Categories.Any())
    {
        var cat = new Domain.Category { Name = "General" };
        db.Categories.Add(cat);
        db.Products.AddRange(
            new Domain.Product { Name = "Camiseta", Sku = "SKU-001", Price = 19.99m, Category = cat },
            new Domain.Product { Name = "Gorra",    Sku = "SKU-002", Price = 12.5m,  Category = cat }
        );
        await db.SaveChangesAsync();
    }
}

app.Run();
