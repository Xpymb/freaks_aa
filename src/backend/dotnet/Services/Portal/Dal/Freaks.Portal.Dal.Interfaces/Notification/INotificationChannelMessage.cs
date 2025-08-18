using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.Notification;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannelMessage;
using Freaks.SharedContracts.Common;

namespace Freaks.Portal.Dal.Interfaces.Notification;

/// <summary>
/// Интерфейс провайдера для работы с сообщениями Discord-каналов.
/// Предоставляет методы для доступа к сообщениям с поддержкой пагинации и сортировки.
/// </summary>
public interface INotificationChannelMessageProvider 
    : IBaseProvider<NotificationChannelMessage, long, IPortalDbContext>
{
    /// <summary>
    /// Сообщение с пагинацией и сортировкой
    /// </summary>
    /// <param name="request">Параметры фильтрации, сортировки и пагинации.</param>
    /// <returns>Возвращает сообщения, соответствующие условиям запроса.</returns>
    Task<PaginatedList<NotificationChannelMessage>> GetPaginatedListAsync(GetNotificationChannelMessageRequest request);
    
}