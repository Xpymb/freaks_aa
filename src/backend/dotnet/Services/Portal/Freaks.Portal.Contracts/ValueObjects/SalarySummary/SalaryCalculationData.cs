using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Contracts.ValueObjects.RaidSummary;

namespace Freaks.Portal.Contracts.ValueObjects.SalarySummary;

/// <summary>
///     Входные данные для алгоритма расчёта зарплат.
/// </summary>
/// <param name="Salary">Зарплатный период.</param>
/// <param name="SalaryLoots">Проданный лут за период.</param>
/// <param name="SalaryGuildLeaderExpanses">Доля руководства гильдии.</param>
/// <param name="SalaryExpenses">Расходы и отчисления.</param>
/// <param name="SalaryMembers">Участники зарплатного периода.</param>
/// <param name="RaidFullInfos">Рейды с полной информацией (участники, лут).</param>
public record SalaryCalculationData(
    Salary Salary,
    IList<SalaryLoot> SalaryLoots,
    IList<SalaryGuildLeader> SalaryGuildLeaderExpanses,
    IList<SalaryExpenses> SalaryExpenses,
    IList<SalaryMember> SalaryMembers,
    IList<RaidFullInfo> RaidFullInfos);
