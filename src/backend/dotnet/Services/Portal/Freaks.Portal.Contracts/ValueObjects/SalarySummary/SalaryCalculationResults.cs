namespace Freaks.Portal.Contracts.ValueObjects.SalarySummary;

/// <summary>
///     Результаты расчёта зарплат за период.
/// </summary>
/// <param name="MembersResult">Результаты расчёта по каждому участнику.</param>
/// <param name="GoldForSalary">Золото, распределённое участникам.</param>
/// <param name="WorldBossInfusionForSalary">Опыт синтеза инфузий мирового босса, распределённый участникам.</param>
/// <param name="WorldBossInfusionForSale">Опыт синтеза инфузий мирового босса, оставшийся на продажу.</param>
/// <param name="ErenorInfusionForSalary">Опыт синтеза эренорских инфузий, распределённый участникам.</param>
/// <param name="ErenorInfusionForSale">Опыт синтеза эренорских инфузий, оставшийся на продажу.</param>
public record SalaryCalculationResults(
    IList<SalaryMemberCalculationResult> MembersResult,
    decimal GoldForSalary,
    decimal WorldBossInfusionForSalary,
    decimal WorldBossInfusionForSale,
    decimal ErenorInfusionForSalary,
    decimal ErenorInfusionForSale);
