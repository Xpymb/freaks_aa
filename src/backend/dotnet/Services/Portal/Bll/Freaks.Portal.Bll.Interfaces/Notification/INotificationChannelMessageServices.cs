using Freaks.Portal.SharedContracts.Dto.Notification;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannelMessage;
using Freaks.SharedContracts.Common;

namespace Freaks.Portal.Bll.Interfaces.Notification;

/// <summary>
/// Сервис для работы с сообщениями Discord-каналов.
/// Предоставляет методы для получения сообщений с пагинацией и создания новых сообщений.
/// </summary>
public interface INotificationChannelMessageService
{
    /// <summary>
    /// Получает пагинированный список сообщений указанного канала.
    /// </summary>
    /// <param name="request">Параметры запроса для пагинации и фильтрации</param>
    /// <returns>Пагинированный список сообщений с метаданными (общее количество, текущая страница).</returns>
    Task<PaginatedList<NotificationChannelMessageDto>> GetListAsync(GetNotificationChannelMessageRequest request);
    
   /// <summary>
   /// Создает новое сообщение в указанном Discord-канале.
   /// </summary>
   /// <param name="notificationChannelId">Идентификатор канала, в который добавляется сообщение.</param>
   /// <param name="request">Запрос на создание сообщения.</param>
   /// <returns>Созданное сообщение в формате DTO.</returns>
    Task<NotificationChannelMessageDto> CreateAsync(int notificationChannelId,CreateNotificationChannelMessageRequest request);
}
