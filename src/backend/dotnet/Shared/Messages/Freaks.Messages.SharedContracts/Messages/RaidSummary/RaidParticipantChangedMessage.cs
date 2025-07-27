using Freaks.Messages.SharedContracts.Attributes;

namespace Freaks.Messages.SharedContracts.Messages.RaidSummary;

/// <summary>
///     Сообщение об изменении состава участников рейда,
///     содержит информацию о выполненном действии и идентификаторе рейда.
/// </summary>
[MessageTopic("raid.participant")]
public class RaidParticipantChangedMessage : BaseMessage
{
    /// <summary>
    ///     Идентификатор рейда, для которого изменился состав участников.
    /// </summary>
    public required long RaidId { get; init; }
}