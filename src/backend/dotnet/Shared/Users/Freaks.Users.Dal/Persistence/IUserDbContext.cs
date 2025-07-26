using Freaks.Dal.Common.Interfaces;
using Freaks.Users.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Users.Dal.Persistence;

/// <summary>
///     Контракт контекста базы данных, связанного с пользователями.
///     Определяет доступ к таблице <see cref="User" />.
///     Наследует базовые возможности контекста из <see cref="IBaseDbContext" />.
/// </summary>
public interface IUserDbContext : IBaseDbContext
{
    /// <summary>
    ///     Набор пользователей в базе данных.
    /// </summary>
    DbSet<User> Users { get; init; }
}