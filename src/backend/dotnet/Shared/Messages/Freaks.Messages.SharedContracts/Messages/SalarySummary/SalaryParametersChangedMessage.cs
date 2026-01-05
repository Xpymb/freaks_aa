using Freaks.Messages.SharedContracts.Attributes;

namespace Freaks.Messages.SharedContracts.Messages.SalarySummary;

/// <summary>
///     Сообщение об изменении параметров зарплатного периода.
///     Публикуется при обновлении настроек расчета зарплаты.
/// </summary>
[MessageTopic("salary.parameters")]
public class SalaryParametersChangedMessage : BaseMessage
{
    /// <summary>
    ///     Идентификатор зарплатного периода, параметры которого изменились.
    /// </summary>
    public required long SalaryId { get; init; }
}
