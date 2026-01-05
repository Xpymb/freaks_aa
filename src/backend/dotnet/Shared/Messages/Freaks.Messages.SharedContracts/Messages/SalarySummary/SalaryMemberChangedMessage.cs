using Freaks.Messages.SharedContracts.Attributes;

namespace Freaks.Messages.SharedContracts.Messages.SalarySummary;

/// <summary>
///     Сообщение о том, что участники зарплатного периода изменились,
///     содержит информацию о действии и идентификаторе зарплатного периода.
/// </summary>
[MessageTopic("salary.member")]
public class SalaryMemberChangedMessage : BaseMessage
{
    /// <summary>
    ///     Идентификатор зарплатного периода, в котором изменились участники.
    /// </summary>
    public required long SalaryId { get; init; }
}
