using Freaks.Messages.SharedContracts.Attributes;

namespace Freaks.Messages.SharedContracts.Messages.SalarySummary;

/// <summary>
///     Сообщение, связанное с сущностью "зарплатный период", содержащее информацию о выполненном действии.
/// </summary>
[MessageTopic("salary")]
public class SalaryChangedMessage : BaseMessage
{
    /// <summary>
    ///     Идентификатор зарплатного периода, к которому относится сообщение.
    /// </summary>
    public required long Id { get; init; }
}
