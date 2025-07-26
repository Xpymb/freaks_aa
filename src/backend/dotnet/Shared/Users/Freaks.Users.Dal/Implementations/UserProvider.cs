using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Users.Contracts.Entities;
using Freaks.Users.Dal.Interfaces;
using Freaks.Users.Dal.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Users.Dal.Implementations;

/// <summary>
///     Провайдер доступа к данным пользователей с поддержкой кэширования.
///     Реализует логику построения ключей и префиксов кэша для сущности <see cref="User" />.
/// </summary>
public class UserProvider : BaseCachedProvider<User, Guid, IUserDbContext>, IUserProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="UserProvider" />.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных пользователей.</param>
    /// <param name="cacheProvider">Провайдер кэширования.</param>
    public UserProvider(IUserDbContext dbContext, ICacheProvider cacheProvider) : base(dbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    public async Task<IList<User>> GetListAsync(bool includeWoRoles)
    {
        var cacheKey = GetDefaultCachePrefix();
        var cachedValue = await GetCachedValueAsync<IList<User>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var query = Set.AsNoTracking();

        if (!includeWoRoles)
        {
            query = query.Where(u => u.Roles.Any());
        }

        var result = await query.ToListAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
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
        return
        [
            GetDefaultCachePrefix(),
        ];
    }

    private static string GetDefaultCachePrefix()
    {
        return $"{nameof(User)}:list";
    }
}