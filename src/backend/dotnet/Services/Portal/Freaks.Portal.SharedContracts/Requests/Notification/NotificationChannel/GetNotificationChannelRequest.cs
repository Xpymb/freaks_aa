namespace Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannel;

/// <summary>
/// Запрос на получение информации о Discord-канале.
/// </summary>
/// <param name="DiscordChannelId">Идентификатор канала в Discord</param>
/// <param name="Name">Название канала для дополнительной фильтрации</param>
public record GetNotificationChannelRequest(
    long DiscordChannelId,
    string  Name);