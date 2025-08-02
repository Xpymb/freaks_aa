using Freaks.Portal.SharedContracts.Dto.Notification;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannelMessage;
using Freaks.SharedContracts.Common;

namespace Freaks.Portal.Bll.Interfaces.Notification;

/// <summary>
/// 
/// </summary>
public interface INotificationChannelMessageService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<PaginatedList<NotificationChannelMessageDto>> GetListAsync(GetNotificationChannelMessageRequest request);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="NotificationChannelId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<NotificationChannelMessageDto> CreateAsync(int NotificationChannelId,CreateNotificationChannelMessageRequest request);
}
