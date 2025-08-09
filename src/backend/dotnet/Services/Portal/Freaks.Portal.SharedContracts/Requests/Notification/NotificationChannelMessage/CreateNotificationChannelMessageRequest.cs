namespace Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannelMessage;

/// <summary>
/// Запрос на создание сообщения в Discord-канале.
/// </summary>
/// <param name="CreatorId">Идентификатор пользователя-автора сообщения</param>
/// <param name="Message">Текст сообщения </param>
/// <param name="CreatedDt">Дата и время создания сообщения</param>
public record CreateNotificationChannelMessageRequest(
    Guid CreatorId,
    string Message,
    DateTimeOffset CreatedDt );