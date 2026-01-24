using Freaks.Portal.Bll.Interfaces.Notification;
using Freaks.Portal.SharedContracts.Dto.Notification;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannel;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.Notification;

/// <summary>
/// Контроллер для работы с Discord-каналами.
/// Предоставляет API для получения списка каналов и создания новых.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("notification-channels")]
public class NotificationChannelController : ControllerBase
{
    private readonly INotificationChannelService _service;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера.
    /// </summary>
    /// <param name="service">Сервис для работы с каналами</param>
    public NotificationChannelController(INotificationChannelService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Получает список всех зарегистрированных каналов.
    /// </summary>
    /// <returns>Список каналов в формате DTO.</returns>
    [HttpGet]
    public Task<IList<NotificationChannelDto>> GetListAsync()
    {
        return _service.GetListAsync();
    }

    /// <summary>
    /// Создает новый канал в системе.
    /// </summary>
    /// <param name="request">Данные для создания канала.</param>
    /// <returns>/// Созданный канал в формате DTO.</returns>
    [RequireRoles(UserRole.Admin,  UserRole.GuildLeader)]
    [HttpPost]
    public Task<NotificationChannelDto> CreateAsync([FromBody] CreateNotificationChannelRequest request)
    {
        return _service.CreateAsync(request);
    }
}
