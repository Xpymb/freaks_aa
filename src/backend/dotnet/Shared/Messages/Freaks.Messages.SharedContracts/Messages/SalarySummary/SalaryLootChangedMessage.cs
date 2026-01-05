using Freaks.Messages.SharedContracts.Attributes;

namespace Freaks.Messages.SharedContracts.Messages.SalarySummary;

/// <summary>
///     Сообщение о том, что содержимое лута в зарплатном периоде изменилось,
///     содержит информацию о действии и идентификаторе зарплатного периода.
/// </summary>
[MessageTopic("salary.loot")]
public class SalaryLootChangedMessage : BaseMessage
{
    /// <summary>
    ///     Идентификатор зарплатного периода, в котором изменился лут.
    /// </summary>
    public required long SalaryId { get; init; }
}
