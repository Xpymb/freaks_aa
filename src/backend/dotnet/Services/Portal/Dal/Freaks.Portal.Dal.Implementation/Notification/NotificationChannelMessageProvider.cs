using System.Text.Json;
using Freaks.Dal.Common.Extensions;
using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.Notification;
using Freaks.Portal.Dal.Interfaces.Notification;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannelMessage;
using Freaks.SharedContracts.Common;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.Notification;

/// <summary>
/// Провайдер доступа к данным сообщений Discord-каналов.
/// Обеспечивает кэшированное взаимодействие с базой данных для операций с сообщениями.
/// </summary>
public class NotificationChannelMessageProvider : BaseCachedProvider<NotificationChannelMessage, long, IPortalDbContext>,
                                                                                     INotificationChannelMessageProvider
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="NotificationChannelMessageProvider"/>.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных портала.</param>
    /// <param name="cacheProvider">Провайдер кэширования.</param>
    public NotificationChannelMessageProvider(IPortalDbContext dbContext,
                                              ICacheProvider cacheProvider) : base(dbContext, cacheProvider)
    {
    }

    /// <inheritdoc />
    public async Task<PaginatedList<NotificationChannelMessage>> GetPaginatedListAsync(
                                                                 GetNotificationChannelMessageRequest request)
    {
        var cacheKey = GetParameterizedCacheKey(request);
        var cachedValue = await GetCachedValueAsync<PaginatedList<NotificationChannelMessage>>(cacheKey);
        
        if (cachedValue is not null)
        {
            return cachedValue;
        }
        
        var query =
             Set.Include(n => n.NotificationChannel)
                .AsNoTracking();

        query = request.SortBy switch
        {
            NotificationChannelMessageSortBy.CreatedDt => query.OrderBy(n => n.CreatedDt, request.SortMode),
            _ => throw new ArgumentOutOfRangeException(),
        };

        var resultCount = await query.CountAsync();
        var result =
            await query.UseTakeSkip(request.Take, request.Skip)
                       .ToListAsync();

        var paginatedResult = new PaginatedList<NotificationChannelMessage>(result,  request.Take, request.Skip, resultCount);
        
        await SetCachedValueAsync(cacheKey, paginatedResult, TimeSpan.FromMinutes(5));
        return paginatedResult;

    }
    
    /// <inheritdoc />
    protected override string GetCacheKey(long key)
    {
        return $"{nameof(NotificationChannelMessage)}:{key}";
    }
    
    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(NotificationChannelMessage entity)
    {
        return
        [
            GetCacheKey(entity.Id),
        ];
    }
    
    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(NotificationChannelMessage entity)
    {
        return
        [
            GetDefaultCachePrefix(),
        ];
    }
    
    /// <summary>
    /// Возвращает стандартный префикс ключа кэша для списка сообщений каналов.
    /// </summary>
    private static string GetDefaultCachePrefix()
    {
        return $"{nameof(NotificationChannelMessage)}:list";
    }
    
    /// <summary>
    /// Генерирует уникальный ключ кэша на основе параметров запроса сообщений.
    /// </summary>
    /// <param name="parameters">Параметры запроса сообщений.</param>
    /// <returns>Уникальный строковый ключ кэша.</returns>
     private static string GetParameterizedCacheKey(GetNotificationChannelMessageRequest parameters)
    {
        var parametersJson = JsonSerializer.Serialize(parameters);
        return $"{GetDefaultCachePrefix()}:{parametersJson}";
    }
}