using Freaks.Messages.SharedContracts.Attributes;

namespace Freaks.Messages.SharedContracts.Messages.Notification;

/// <summary>
/// Сообщение об изменении состояния канала.
/// </summary>
[MessageTopic("notification.channel")]
public class NotificationChannelChangedMessage : BaseMessage
{
    /// <summary>
    /// Внутренний идентификатор канала в системе.
    /// </summary>
    public required long Id { get; init; }
} 