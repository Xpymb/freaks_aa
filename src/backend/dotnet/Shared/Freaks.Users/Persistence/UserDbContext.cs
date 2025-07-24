using Freaks.Users.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Users.Persistence;

/// <summary>
///     Контекст базы данных для работы с пользователями.
///     Содержит конфигурации сущностей и доступ к таблице пользователей.
/// </summary>
public class UserDbContext : DbContext, IUserDbContext
{
    /// <summary>
    ///     Инициализирует новый экземпляр контекста <see cref="UserDbContext" /> с заданными параметрами.
    /// </summary>
    /// <param name="options">Опции конфигурации контекста.</param>
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc />
    public DbSet<User> Users { get; init; }
}