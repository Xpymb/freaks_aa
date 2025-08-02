using System.Text.Json;
using Freaks.Dal.Common.Extensions;
using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Contracts.Entities.Notification;
using Freaks.Portal.Dal.Interfaces.Notification;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannelMessage;
using Freaks.SharedContracts.Common;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.Notification;

public class NotificationChannelMessageProvider : BaseCachedProvider<NotificationChannelMessage, long, IPortalDbContext>,
                                                                                     INotificationChannelMessageProvider
{
    public NotificationChannelMessageProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider) : base(dbContext, cacheProvider)
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

        query = request.sortBy switch
        {
            NotificationChannelMessageSortBy.CreatedDt => query.OrderBy(n => n.CreatedDt, request.SortMode),
            _ => throw new ArgumentOutOfRangeException(),
        };

        var result =
            await query.UseTakeSkip(request.Take, request.Skip)
                       .ToListAsync();
        
        var resultCount = await query.CountAsync();
        var pagintatedResult = new PaginatedList<NotificationChannelMessage>(result,  request.Take, request.Skip, resultCount);
        
        await SetCachedValueAsync(cacheKey, pagintatedResult, TimeSpan.FromMinutes(5));
        return pagintatedResult;

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
    
    private static string GetDefaultCachePrefix()
    {
        return $"{nameof(NotificationChannelMessage)}:list";
    }
    
     private static string GetParameterizedCacheKey(GetNotificationChannelMessageRequest parameters)
    {
        var parametersJson = JsonSerializer.Serialize(parameters);
        return $"{GetDefaultCachePrefix()}:{parametersJson}";
    }
    
}