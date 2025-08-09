namespace Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannel;

/// <summary>
/// Запрос на создание нового Discord-канала в системе.
/// </summary>
/// <param name="DiscordChannelId">Уникальный идентификатор канала в Discord </param>
/// <param name="Name">Название канала </param>
public record CreateNotificationChannelRequest(
    long DiscordChannelId,
    string Name );