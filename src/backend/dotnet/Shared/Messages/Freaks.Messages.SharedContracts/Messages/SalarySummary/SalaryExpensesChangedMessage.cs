using Freaks.Messages.SharedContracts.Attributes;

namespace Freaks.Messages.SharedContracts.Messages.SalarySummary;

/// <summary>
///     Сообщение о том, что содержимое расходов гильдии в зарплатном периоде изменилось,
///     содержит информацию о действии и идентификаторе зарплатного периода.
/// </summary>
[MessageTopic("salary.expenses")]
public class SalaryExpensesChangedMessage : BaseMessage
{
    /// <summary>
    ///     Идентификатор статьи расхода.
    /// </summary>
    public required long Id { get; init; }
}
