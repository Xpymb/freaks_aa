using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.Loot;
using Freaks.Portal.Dal.Interfaces.Loot;
using Freaks.Portal.Dal.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.Loot;

/// <summary>
///     Провайдер для получения предметов добычи с кэшированием.
///     Наследует базовую реализацию с поддержкой кэша из <see cref="BaseCachedProvider{TEntity,TKey,TContext}" />.
/// </summary>
public class LootItemProvider : BaseCachedProvider<LootItem, int, IPortalDbContext>, ILootItemProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="LootItemProvider" />.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных портала.</param>
    /// <param name="cacheProvider">Провайдер кэширования.</param>
    public LootItemProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider) : base(dbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    public async Task<IList<LootItem>> GetListAsync()
    {
        var cacheKey = GetDefaultCachePrefix();
        var cachedValue = await GetCachedValueAsync<IList<LootItem>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result =
            await Set
                  .AsNoTracking()
                  .ToListAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromHours(1));
        return result;
    }

    /// <inheritdoc />
    protected override string GetCacheKey(int key)
    {
        return $"{nameof(LootItem)}:{key}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(LootItem entity)
    {
        return
        [
            GetCacheKey(entity.Id),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(LootItem entity)
    {
        return
        [
            GetDefaultCachePrefix(),
        ];
    }

    /// <summary>
    ///     Возвращает префикс для кэширования списка всех предметов добычи.
    /// </summary>
    /// <returns>Строковый префикс кэша.</returns>
    private static string GetDefaultCachePrefix()
    {
        return $"{nameof(LootItem)}:list";
    }
}