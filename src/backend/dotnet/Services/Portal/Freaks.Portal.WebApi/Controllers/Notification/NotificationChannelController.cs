using Freaks.Portal.Bll.Interfaces.Notification;
using Freaks.Portal.SharedContracts.Dto.Notification;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannel;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.Notification;

/// <summary>
/// 
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("notification-channels")]
public class NotificationChannelController : ControllerBase
{
    private readonly INotificationChannelService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public NotificationChannelController(INotificationChannelService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IList<NotificationChannelDto>> GetListAsync()
    {
        return await _service.GetListAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [RequireRoles(UserRole.Admin,  UserRole.GuildLeader)]
    [HttpPost]
    public async Task<NotificationChannelDto> CreateAsync([FromBody] CreateNotificationChannelRequest request)
    {
        return await _service.CreateAsync(request);
    }
}
