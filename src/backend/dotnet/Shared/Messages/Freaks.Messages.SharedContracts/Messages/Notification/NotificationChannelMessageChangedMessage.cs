using Freaks.Messages.SharedContracts.Attributes;

namespace Freaks.Messages.SharedContracts.Messages.Notification;

/// <summary>
/// Сообщение об изменении состояния сообщения в канале.
/// </summary>
[MessageTopic("notification.channel.message")]
public class NotificationChannelMessageChangedMessage : BaseMessage
{
    /// <summary>
    /// Внутренний идентификатор канала, содержащего сообщение.
    /// </summary>
    public required long ChannelId { get; init; }
    
    /// <summary>
    /// Внутренний идентификатор измененного сообщения.
    /// </summary>
    public required long MessageId { get; init; }
}  