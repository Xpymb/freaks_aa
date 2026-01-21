using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;
using Freaks.Users.SharedContracts.Dto;

namespace Freaks.Portal.SharedContracts.Dto.SalarySummary;

/// <summary>
///     DTO, представляющее участника зарплатного периода.
/// </summary>
/// <param name="SalaryId">Идентификатор зарплатного периода.</param>
/// <param name="User">Пользователь.</param>
/// <param name="PaymentType">Тип выплаты.</param>
/// <param name="ActivityPercentage">Процент активности.</param>
/// <param name="Coefficient">Коэффициент.</param>
/// <param name="ActivityGold">Золото за активность.</param>
/// <param name="ResponsibilityGold">Золото за ответственность.</param>
/// <param name="AmountGold">Общая сумма золота.</param>
/// <param name="AmountWorldBossInfusion">Количество инфузий мирового босса.</param>
public record SalaryMemberDto(
    long SalaryId,
    UserDto User,
    SalaryPaymentType PaymentType,
    decimal ActivityPercentage,
    decimal? Coefficient,
    decimal? ActivityGold,
    decimal? ResponsibilityGold,
    decimal? AmountGold,
    decimal? AmountWorldBossInfusion);
