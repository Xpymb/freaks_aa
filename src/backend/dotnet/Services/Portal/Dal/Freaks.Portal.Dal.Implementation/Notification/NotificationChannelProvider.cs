using Freaks.Dal.Common.Implementations;
using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Contracts.Entities.Notification;
using Freaks.Portal.Dal.Interfaces.Notification;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannel;
using Microsoft.EntityFrameworkCore;

namespace Freaks.Portal.Dal.Implementation.Notification;

public class NotificationChannelProvider : BaseCachedProvider<NotificationChannel, int, IPortalDbContext>,
                                                                                     INotificationChannelProvider
{
    public NotificationChannelProvider(IPortalDbContext dbContext, ICacheProvider cacheProvider) : base(dbContext, cacheProvider)
    {
    }
    
    /// <inheritdoc />
    public async Task<IList<NotificationChannel>> GetListAsync()
    {
        var cacheKey = GetDefaultCachePrefix();
        var cachedValue = await GetCachedValueAsync<IList<NotificationChannel>>(cacheKey);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result =
            await Set
                .AsNoTracking()
                .ToListAsync();

        await SetCachedValueAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }
    
    /// <inheritdoc />
    protected override string GetCacheKey(int key)
    {
        return $"{nameof(NotificationChannel)}:{key}";
    }
    
    /// <inheritdoc />
    protected override List<string> GetAllCacheKeys(NotificationChannel entity)
    {
        return
        [
            GetCacheKey(entity.Id),
        ];
    }
    
    /// <inheritdoc />
    protected override List<string> GetAllCachePrefixes(NotificationChannel entity)
    {
        return
        [
            GetDefaultCachePrefix(),
        ];
    }
    
    private static string GetDefaultCachePrefix()
    {
        return $"{nameof(NotificationChannel)}:list";
    }
    
}