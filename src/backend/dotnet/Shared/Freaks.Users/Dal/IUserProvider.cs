using Freaks.Dal.Common.Interfaces;
using Freaks.Users.Contracts;
using Freaks.Users.Persistence;

namespace Freaks.Users.Dal;

/// <summary>
///     Провайдер для доступа к данным пользователей.
///     Наследует стандартные CRUD-операции из <see cref="IBaseProvider{TEntity,TKey,TContext}" />.
/// </summary>
public interface IUserProvider : IBaseProvider<User, Guid, UserDbContext>
{
    /// <summary>
    ///     Возвращает список всех пользователей из базы данных.
    /// </summary>
    /// <returns>Список сущностей <see cref="User" />.</returns>
    Task<IList<User>> GetListAsync(bool includeWoRoles);
}