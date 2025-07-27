using Freaks.Messages.SharedContracts.Attributes;

namespace Freaks.Messages.SharedContracts.Messages.RaidSummary;

/// <summary>
///     Сообщение об изменении скриншотов рейда,
///     содержит информацию о выполненном действии и идентификаторе рейда.
/// </summary>
[MessageTopic("raid.screenshot")]
public class RaidScreenshotChangedMessage : BaseMessage
{
    /// <summary>
    ///     Идентификатор рейда, для которого изменился скриншот.
    /// </summary>
    public required long RaidId { get; init; }
}