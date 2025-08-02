namespace Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannelMessage;

/// <summary>
/// 
/// </summary>
/// <param name="NotificationChannelId"></param>
/// <param name="CreatorId"></param>
/// <param name="Message"></param>
/// <param name="CreatedDt"></param>
public record CreateNotificationChannelMessageRequest(
    Guid CreatorId,
    string Message,
    DateTimeOffset CreatedDt );