using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Users.Contracts;

namespace Freaks.Users.Dal;

/// <summary>
///     Провайдер доступа к данным пользователей с поддержкой кэширования.
///     Реализует логику построения ключей и префиксов кэша для сущности <see cref="User" />.
/// </summary>
public class UserProvider : BaseCachedProvider<User, Guid, UserDbContext>, IUserProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="UserProvider" />.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных пользователей.</param>
    /// <param name="cacheProvider">Провайдер кэширования.</param>
    public UserProvider(UserDbContext dbContext, ICacheProvider cacheProvider) : base(dbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    protected override string GetCacheKey(Guid key)
    {
        return $"{nameof(User)}:{key}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(User entity)
    {
        return
        [
            GetCacheKey(entity.Id),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(User entity)
    {
        return [];
    }
}