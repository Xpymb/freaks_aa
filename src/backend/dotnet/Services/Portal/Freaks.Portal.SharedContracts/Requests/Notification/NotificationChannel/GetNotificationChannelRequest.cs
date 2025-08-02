namespace Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannel;

/// <summary>
/// 
/// </summary>
/// <param name="DiscordChannelId"></param>
/// <param name="Name"></param>
public record GetNotificationChannelRequest(
    long DiscordChannelId,
    string  Name);