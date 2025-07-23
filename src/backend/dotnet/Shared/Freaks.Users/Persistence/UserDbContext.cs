using Freaks.Users.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Users.Persistence;

public class UserDbContext : DbContext, IUserDbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; init; }
}