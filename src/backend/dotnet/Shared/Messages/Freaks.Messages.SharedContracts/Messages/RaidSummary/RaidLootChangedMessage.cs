using Freaks.Messages.SharedContracts.Attributes;

namespace Freaks.Messages.SharedContracts.Messages.RaidSummary;

/// <summary>
///     Сообщение о том, что содержимое лута в рейде изменилось,
///     содержит информацию о действии и идентификаторе рейда.
/// </summary>
[MessageTopic("raid.loot")]
public class RaidLootChangedMessage : BaseMessage
{
    /// <summary>
    ///     Идентификатор рейда, в котором изменился лут.
    /// </summary>
    public required long RaidId { get; init; }
}