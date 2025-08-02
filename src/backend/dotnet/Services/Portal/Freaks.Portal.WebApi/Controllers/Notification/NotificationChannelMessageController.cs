using Freaks.Portal.Bll.Interfaces.Notification;
using Freaks.Portal.SharedContracts.Dto.Notification;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannelMessage;
using Freaks.SharedContracts.Common;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.Notification;

[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("notification-channels/{channelId:int}/messages")]
public class NotificationChannelMessageController : ControllerBase
{
    private readonly INotificationChannelMessageService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public NotificationChannelMessageController(INotificationChannelMessageService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PaginatedList<NotificationChannelMessageDto>> GetListAsync(
                 [FromQuery]GetNotificationChannelMessageRequest request)
    {
        return await _service.GetListAsync(request);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="channelId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<NotificationChannelMessageDto> CreateAsync(
                 [FromRoute] int channelId,
                 [FromBody] CreateNotificationChannelMessageRequest request)
    {
        var result = await _service.CreateAsync(channelId, request);
        return result;
    }
}
