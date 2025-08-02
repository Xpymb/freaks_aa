using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.Notification;
using Freaks.Portal.Dal.Persistence;
namespace Freaks.Portal.Dal.Interfaces.Notification;

public interface INotificationChannelProvider 
    : IBaseProvider<NotificationChannel, int, IPortalDbContext>
{
    /// <summary>
    /// Список существующих каналов
    /// </summary>
    /// <returns></returns>
    Task<IList<NotificationChannel>> GetListAsync();
    
}