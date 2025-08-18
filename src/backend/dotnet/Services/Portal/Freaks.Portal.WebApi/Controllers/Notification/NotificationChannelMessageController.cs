using Freaks.Portal.Bll.Interfaces.Notification;
using Freaks.Portal.SharedContracts.Dto.Notification;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannelMessage;
using Freaks.SharedContracts.Common;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.Notification;

/// <summary>
/// Контроллер для работы с сообщениями каналов.
/// Предоставляет API для получения сообщений с пагинацией и сортировкой.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("notification-channels/{channelId:int}/messages")]
public class NotificationChannelMessageController : ControllerBase
{
    private readonly INotificationChannelMessageService _service;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера.
    /// </summary>
    /// <param name="service">Сервис для работы с сообщениями каналов</param>
    public NotificationChannelMessageController(INotificationChannelMessageService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Получает пагинированный список сообщений указанного канала.
    /// </summary>
    /// <param name="request">Параметры фильтрации, сортировки и пагинации.</param>
    /// <returns>Список сообщений.</returns>
    [HttpGet]
    public async Task<PaginatedList<NotificationChannelMessageDto>> GetListAsync(
                 [FromQuery]GetNotificationChannelMessageRequest request)
    {
        return await _service.GetListAsync(request);
    }
}
