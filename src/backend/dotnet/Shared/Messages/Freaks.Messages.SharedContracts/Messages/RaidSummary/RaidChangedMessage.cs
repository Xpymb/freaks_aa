using Freaks.Messages.SharedContracts.Attributes;

namespace Freaks.Messages.SharedContracts.Messages.RaidSummary;

/// <summary>
///     Сообщение, связанное с сущностью "рейд", содержащее информацию о выполненном действии и полезную нагрузку.
/// </summary>
[MessageTopic("raid")]
public class RaidChangedMessage : BaseMessage
{
    /// <summary>
    ///     Идентификатор рейда, к которому относится сообщение.
    /// </summary>
    public required long Id { get; init; }
}