using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.Dal.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.RaidSummary;

/// <summary>
///     Провайдер для работы с участниками рейдов (<see cref="RaidParticipant"/>), включая кэширование по рейдам и пользователям.
/// </summary>
public class RaidParticipantProvider : BaseCachedCompositeProvider<RaidParticipant, RaidParticipantKey, IPortalDbContext>, IRaidParticipantProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="RaidParticipantProvider"/>.
    /// </summary>
    /// <param name="cacheProvider">Провайдер кэша.</param>
    /// <param name="dbContext">Контекст базы данных портала.</param>
    public RaidParticipantProvider(
        ICacheProvider cacheProvider,
        IPortalDbContext dbContext) : base(dbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    public override async Task<RaidParticipant?> GetAsync(RaidParticipantKey key, EntityTrackingType trackingType)
    {
        var cachedValue = await GetCachedValueAsync(key);
        if (cachedValue is not null)
        {
            if (trackingType is EntityTrackingType.Tracking)
            {
                Set.Attach(cachedValue);
            }

            return cachedValue;
        }

        var query = FilterByKey(key, Set);

        if (trackingType is EntityTrackingType.NoTracking)
        {
            query = query.AsNoTracking();
        }

        var result =
            await query
                  .Include(p => p.Participant)
                  .FirstOrDefaultAsync();

        await SetCachedValueAsync(result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<IList<RaidParticipant>> GetByRaidIdAsync(long raidId)
    {
        var cacheKey = GetCacheRaidKey(raidId);
        var cachedValue = await GetCachedValueAsync<IList<RaidParticipant>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result =
            await Set
                  .Include(p => p.Participant)
                  .AsNoTracking()
                  .Where(p => p.RaidId == raidId)
                  .ToListAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<IList<RaidParticipant>> GetByUserIdAsync(Guid userId)
    {
        var cacheKey = GetCacheUserKey(userId);
        var cachedValue = await GetCachedValueAsync<IList<RaidParticipant>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result =
            await Set
                  .Include(p => p.Participant)
                  .AsNoTracking()
                  .Where(p => p.ParticipantId == userId)
                  .ToListAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    protected override IQueryable<RaidParticipant> FilterByKey(RaidParticipantKey key, IQueryable<RaidParticipant> queryable)
    {
        return queryable.Where(p => (p.RaidId == key.RaidId) && (p.ParticipantId == key.ParticipantId));
    }

    /// <inheritdoc />
    protected override string GetCacheKey(RaidParticipantKey key)
    {
        return $"{nameof(RaidParticipant)}:raid:{key.RaidId}:participant:{key.ParticipantId}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(RaidParticipant entity)
    {
        return
        [
            GetCacheKey(entity.GetCompositeKey()),
            GetCacheRaidKey(entity.RaidId),
            GetCacheUserKey(entity.ParticipantId),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(RaidParticipant entity)
    {
        return [];
    }
    
    /// <summary>
    ///     Генерирует ключ кэша для списка участников конкретного рейда.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Строковой ключ кэша.</returns>
    private static string GetCacheRaidKey(long raidId)
    {
        return $"{nameof(RaidParticipant)}:list:raid:{raidId}";
    }

    /// <summary>
    ///     Генерирует ключ кэша для списка участий конкретного пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Строковой ключ кэша.</returns>
    private static string GetCacheUserKey(Guid userId)
    {
        return $"{nameof(RaidParticipant)}:list:user:{userId}";
    }
}