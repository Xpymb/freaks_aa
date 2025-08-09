using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.Notification;
using Freaks.Portal.Dal.Persistence;
namespace Freaks.Portal.Dal.Interfaces.Notification;

/// <summary>
/// Интерфейс провайдера для работы с Discord-каналами в базе данных.
/// Предоставляет методы для доступа и управления информацией о каналах Discord
/// </summary>
public interface INotificationChannelProvider 
    : IBaseProvider<NotificationChannel, int, IPortalDbContext>
{
    /// <summary>
    /// Список существующих каналов
    /// </summary>
    /// <returns>Возвращает список каналов</returns>
    Task<IList<NotificationChannel>> GetListAsync();
}