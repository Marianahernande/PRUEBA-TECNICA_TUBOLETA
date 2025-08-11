using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ECommerce.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // OJO: usar 127.0.0.1 y versión explícita para NO usar AutoDetect durante diseño
        var cs = "Server=127.0.0.1;Port=3306;Database=ecommerce;User=ecommerce;Password=ecommerce123;";
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 36)); // cualquier 8.0.x estable

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(cs, serverVersion)
            .Options;

        return new AppDbContext(options);
    }
}
