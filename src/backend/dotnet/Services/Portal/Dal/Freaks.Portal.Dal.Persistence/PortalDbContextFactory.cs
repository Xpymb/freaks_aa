using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Freaks.Portal.Dal.Persistence;

public sealed class PortalDbContextFactory : IDesignTimeDbContextFactory<PortalDbContext>
{
    public PortalDbContext CreateDbContext(string[] args)
    {
        var cs = Environment.GetEnvironmentVariable("DB_CONNECTION")
                 ?? "Host=127.0.0.1;Port=5432;Username=postgres;Password=postgres;Database=dummy";

        var opts = new DbContextOptionsBuilder<PortalDbContext>()
            .UseNpgsql(cs, b => b.MigrationsAssembly(typeof(PortalDbContext).Assembly.FullName))
            .Options;

        return new PortalDbContext(opts);
    }
}