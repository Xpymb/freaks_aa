using Freaks.Dal.Common.Interfaces;
using Freaks.Users.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Users.Dal;

public interface IUserDbContext : IBaseDbContext
{
    DbSet<User> Users { get; init; }
}