using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.Notification;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannelMessage;
using Freaks.SharedContracts.Common;

namespace Freaks.Portal.Dal.Interfaces.Notification;

public interface INotificationChannelMessageProvider 
    : IBaseProvider<NotificationChannelMessage, long, IPortalDbContext>
{
    /// <summary>
    /// Сообщение с пагинацией и сортировкой
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<PaginatedList<NotificationChannelMessage>> GetPaginatedListAsync(GetNotificationChannelMessageRequest request);
    
}