using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryMember;

/// <summary>
///     Запрос на создание нового участника зарплатного периода (для Admin/Editor/GuildLeader).
///     Admin/Editor/GuildLeader могут создать участника для любого пользователя.
/// </summary>
/// <param name="UserId">Идентификатор пользователя.</param>
/// <param name="PaymentType">Тип выплаты.</param>
/// <param name="ActivityPercentage">Процент активности.</param>
/// <param name="Coefficient">Коэффициент.</param>
public record CreateSalaryMemberAdminRequest(
    Guid UserId,
    SalaryPaymentType PaymentType,
    decimal ActivityPercentage,
    decimal? Coefficient);
