using Freaks.Portal.SharedContracts.Dto.Notification;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannel;

namespace Freaks.Portal.Bll.Interfaces.Notification;


public interface INotificationChannelService
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IList<NotificationChannelDto>> GetListAsync();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<NotificationChannelDto> CreateAsync(CreateNotificationChannelRequest request);
}

