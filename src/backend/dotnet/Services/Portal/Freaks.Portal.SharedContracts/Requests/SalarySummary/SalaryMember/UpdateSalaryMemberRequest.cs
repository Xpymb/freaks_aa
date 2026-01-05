using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryMember;

/// <summary>
///     Запрос на обновление информации об участнике зарплатного периода.
/// </summary>
/// <param name="PaymentType">Тип выплаты.</param>
/// <param name="ActivityPercentage">Процент активности.</param>
/// <param name="Coefficient">Коэффициент.</param>
public record UpdateSalaryMemberRequest(
    SalaryPaymentType PaymentType,
    decimal ActivityPercentage,
    decimal? Coefficient);
