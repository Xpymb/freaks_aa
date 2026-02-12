namespace Freaks.Portal.Contracts.ValueObjects.SalarySummary;

/// <summary>
///     Результат расчёта зарплаты для отдельного участника.
/// </summary>
/// <param name="MemberId">Идентификатор пользователя.</param>
/// <param name="ActivityPercentage">Процент активности (доля посещённых рейдов).</param>
/// <param name="ActivityGold">Золото, начисленное за активность (участие в рейдах).</param>
/// <param name="ResponsibilityGold">Золото, начисленное за ответственность.</param>
/// <param name="AmountGold">Итоговая сумма к выплате в золоте.</param>
/// <param name="AmountWorldBossInfusion">Итоговая сумма к выплате в опыте синтеза инфузий мирового босса.</param>
/// <param name="AmountErenorInfusion">Итоговая сумма к выплате в опыте синтеза эренорских инфузий.</param>
public record SalaryMemberCalculationResult(
    Guid MemberId,
    decimal ActivityPercentage,
    decimal ActivityGold,
    decimal ResponsibilityGold,
    decimal AmountGold,
    decimal AmountWorldBossInfusion,
    decimal AmountErenorInfusion);
