using System.Text.Json;
using Freaks.Dal.Common.Extensions;
using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.Raid;
using Freaks.SharedContracts.Common;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.RaidSummary;

/// <summary>
///     Провайдер для работы с сущностью <see cref="Raid"/>, включающий поддержку кэширования.
///     Позволяет получать рейды по идентификатору и выполнять постраничную выборку с фильтрацией и сортировкой.
/// </summary>
public class RaidProvider : BaseCachedProvider<Raid, long, IPortalDbContext>, IRaidProvider
{
    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="RaidProvider"/>.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных портала.</param>
    /// <param name="cacheProvider">Провайдер кэширования.</param>
    public RaidProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider) : base(dbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    public override async Task<Raid?> GetAsync(long key, EntityTrackingType trackingType)
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

        var query =
            Set
                .Include(r => r.Creator)
                .Where(r => r.Id == key);

        if (trackingType is EntityTrackingType.NoTracking)
        {
            query = query.AsNoTracking();
        }

        var result = await query.FirstOrDefaultAsync();

        await SetCachedValueAsync(result, TimeSpan.FromMinutes(5));
        return result;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<Raid>> GetPaginatedListAsync(GetRaidListRequest request)
    {
        var cacheKey = GetParameterizedCacheKey(request);
        var cachedValue = await GetCachedValueAsync<PaginatedList<Raid>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var query =
            Set
                .Include(r => r.Creator)
                .AsNoTracking();

        if (request.BossTypes is not null
            && (request.BossTypes.Count > 0))
        {
            query = query.Where(r => request.BossTypes.Contains(r.BossType));
        }

        if (request.FormatTypes is not null
            && (request.FormatTypes.Count > 0))
        {
            query = query.Where(r => (r.FormatType != null) && request.FormatTypes.Contains(r.FormatType.Value));
        }

        if (request.Statuses is not null
            && (request.Statuses.Count > 0))
        {
            query = query.Where(r => request.Statuses.Contains(r.Status));
        }

        if (request.From is not null)
        {
            var fromUtc = new DateTimeOffset(
                request.From.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
            );
            query = query.Where(r => r.StartDt >= fromUtc);
        }

        if (request.To is not null)
        {
            var toUtcExclusive = new DateTimeOffset(
                request.To.Value.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
            );
            query = query.Where(r => r.StartDt < toUtcExclusive);
        }

        query =
            request.SortBy switch
            {
                RaidListSortByType.StartDt => query.OrderBy(r => r.StartDt, request.SortMode),
                RaidListSortByType.RaidStatus => query.OrderBy(r => r.Status, request.SortMode),
                RaidListSortByType.CreatedDt => query.OrderBy(r => r.CreatedDt, request.SortMode),
                _ => throw new ArgumentOutOfRangeException(),
            };

        var resultCount = await query.CountAsync();
        var result =
            await query
                  .UseTakeSkip(request.Take, request.Skip)
                  .ToListAsync();

        var paginatedResult = new PaginatedList<Raid>(result, request.Take, request.Skip, resultCount);

        await SetCachedValueAsync(cacheKey, paginatedResult, TimeSpan.FromMinutes(5));
        return paginatedResult;
    }

    /// <inheritdoc />
    protected override string GetCacheKey(long key)
    {
        return $"{nameof(Raid)}:{key}";
    }

    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(Raid entity)
    {
        return
        [
            GetCacheKey(entity.Id),
        ];
    }

    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(Raid entity)
    {
        return
        [
            GetDefaultCachePrefix(),
        ];
    }

    /// <summary>
    ///     Возвращает стандартный префикс кэша для запросов списка рейдов.
    /// </summary>
    private static string GetDefaultCachePrefix()
    {
        return $"{nameof(Raid)}:list";
    }

    /// <summary>
    ///     Генерирует параметризированный ключ кэша на основе запроса <see cref="GetRaidListRequest"/>.
    /// </summary>
    /// <param name="parameters">Параметры запроса списка рейдов.</param>
    /// <returns>Строковой ключ кэша.</returns>
    private static string GetParameterizedCacheKey(GetRaidListRequest parameters)
    {
        var parametersJson = JsonSerializer.Serialize(parameters);
        return $"{GetDefaultCachePrefix()}:{parametersJson}";
    }
}