using Freaks.Messages.SharedContracts.Attributes;

namespace Freaks.Messages.SharedContracts.Messages.SalarySummary;

/// <summary>
///     Сообщение о том, что содержимое долей руководства гильдии в зарплатном периоде изменилось,
///     содержит информацию о действии и идентификаторе зарплатного периода.
/// </summary>
[MessageTopic("salary.guild_leader")]
public class SalaryGuildLeaderChangedMessage : BaseMessage
{
    /// <summary>
    ///     Идентификатор зарплатного периода, в котором изменились доли руководства гильдии.
    /// </summary>
    public required long SalaryId { get; init; }
}
