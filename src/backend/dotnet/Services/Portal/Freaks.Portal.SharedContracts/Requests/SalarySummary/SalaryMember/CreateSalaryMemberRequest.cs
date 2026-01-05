using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryMember;

/// <summary>
///     Запрос на создание нового участника зарплатного периода (для Member).
///     UserId берётся из IUserContext (текущий пользователь).
/// </summary>
/// <param name="PaymentType">Тип выплаты.</param>
/// <param name="ActivityPercentage">Процент активности.</param>
/// <param name="Coefficient">Коэффициент.</param>
public record CreateSalaryMemberRequest(
    SalaryPaymentType PaymentType,
    decimal ActivityPercentage,
    decimal? Coefficient);
