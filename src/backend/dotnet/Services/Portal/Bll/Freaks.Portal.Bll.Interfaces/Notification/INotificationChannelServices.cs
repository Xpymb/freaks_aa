using Freaks.Portal.SharedContracts.Dto.Notification;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannel;

namespace Freaks.Portal.Bll.Interfaces.Notification;

/// <summary>
/// Сервис для управления Discord-каналами.
/// Предоставляет методы для получения списка каналов и создания новых каналов.
/// </summary>
public interface INotificationChannelService
{
    /// <summary>
    /// Получает список всех зарегистрированных Discord-каналов.
    /// </summary>
    /// <returns>Список каналов в формате DTO, содержащий базовую информацию о каждом канале.</returns>
    Task<IList<NotificationChannelDto>> GetListAsync();
    
    /// <summary>
    /// Создает новый Discord-канал в системе на основе переданных данных.
    /// </summary>
    /// <param name="request">Запрос на создание канала.</param>
    /// <returns>Созданный канал в формате DTO.</returns>
    Task<NotificationChannelDto> CreateAsync(CreateNotificationChannelRequest request);
}

