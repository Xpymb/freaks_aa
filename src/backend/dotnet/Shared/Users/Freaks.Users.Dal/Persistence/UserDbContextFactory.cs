using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Freaks.Users.Dal.Persistence;

public sealed class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        var cs = Environment.GetEnvironmentVariable("DB_CONNECTION")
                 ?? "Host=127.0.0.1;Port=5432;Username=postgres;Password=postgres;Database=dummy";

        var opts = new DbContextOptionsBuilder<UserDbContext>()
            .UseNpgsql(cs, b => b.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName))
            .Options;

        return new UserDbContext(opts);
    }
}